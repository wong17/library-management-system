import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ControlStateMatcher } from '../../../../util/control-state-matcher';
import { MonographDto } from '../../../../entities/dtos/library/monograph-dto';
import { MonographAuthorInsertDto } from '../../../../entities/dtos/library/monograph-author-insert-dto';
import { AuthorDto } from '../../../../entities/dtos/library/author-dto';
import { ToastrService } from 'ngx-toastr';
import { MonographAuthorService } from '../../../../services/library/monograph-author.service';
import { AuthorService } from '../../../../services/library/author.service';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { ApiResponse } from '../../../../entities/api/api-response';

@Component({
  selector: 'app-admin-monographs-authors-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatInputModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './admin-monographs-authors-dialog.component.html',
  styleUrl: './admin-monographs-authors-dialog.component.css'
})
export class AdminMonographsAuthorsDialogComponent {

  /* Referencia del formulario */
  monographAuthorsForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()

  monographAuthorInsertDto: MonographAuthorInsertDto[] = []
  monographDto: MonographDto | undefined;
  authors: AuthorDto[] | undefined;

  constructor(
    public dialogRef: MatDialogRef<AdminMonographsAuthorsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private authorService: AuthorService,
    private monographAuthorService: MonographAuthorService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.monographAuthorsForm = this.formBuilder.group({
      authorIds: ['', [Validators.required]]
    });

    /* Obtener autores */
    this.getAuthorsDto()

    // Cargar datos del dto para luego editarlo
    if (dialogData.data) {
      // Obtener dto
      this.monographDto = dialogData.data as MonographDto;
      // Asignar valores en el formulario
      this.monographAuthorsForm.patchValue({
        authorIds: this.monographDto.authors?.map(a => a.authorId) // marcamos los autores de la monografia
      })
    }
  }

  /* Guardar o actualizar */
  onSubmit() {
    // save 
    if (this.dialogData.operation === DialogOperation.Add) {
      this.save();
      return;
    }
    // update
    this.update();
  }

  /* Guardar */
  private save(): void {
    const monographId = this.monographDto?.monographId
    if (!monographId) {
      this.toastr.error(`Ocurrio un error al obtener Id de la monografía`, 'Error', {
        timeOut: 5000,
        easeTime: 1000
      })
      return;
    }
    // obtener autores seleccionados
    const selectedAuthors = this.authorIds?.value as number[];
    selectedAuthors.forEach(aId => {
      this.monographAuthorInsertDto.push({ monographId: monographId, authorId: aId })
    })
    
    // Realizar solicitud http
    this.monographAuthorService.createMany(this.monographAuthorInsertDto).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
        }
        // Solicitud exitosa
        this.toastr.success(`${response.message}`, 'Exito', {
          timeOut: 5000,
          easeTime: 1000
        })
        //
        this.closeDialog(true);
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      }
    })
  }

  /* Actualizar */
  private update(): void {
    const monographId = this.monographDto?.monographId
    if (!monographId) {
      this.toastr.error(`Ocurrio un error al obtener Id de la monografía`, 'Error', {
        timeOut: 5000,
        easeTime: 1000
      })
      return;
    }
    // obtener autores seleccionados
    const selectedAuthors = this.authorIds?.value as number[];
    selectedAuthors.forEach(aId => {
      this.monographAuthorInsertDto.push({ monographId: monographId, authorId: aId })
    })

    // Realizar solicitud http
    this.monographAuthorService.updateMany(this.monographAuthorInsertDto).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
        }
        // Solicitud exitosa
        this.toastr.success(`${response.message}`, 'Exito', {
          timeOut: 5000,
          easeTime: 1000
        })
        //
        this.closeDialog(true);
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      }
    })
  }

  private getAuthorsDto(): void {
    this.authorService.getAll().subscribe({
      next: response => {
        // Verificar si ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
          return;
        }
        // Verificar si el resultado es un array válido
        const list = response.result;
        if (!Array.isArray(list)) {
          this.toastr.error(`El resultado no es un array válido: ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
          return;
        }
        // Asignar datos
        this.authors = list as AuthorDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      },
      complete: () => {
      }
    })
  }

  /* Getters */
  get authorIds() {
    return this.monographAuthorsForm.get('authorIds');
  }

  /* */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }

}
