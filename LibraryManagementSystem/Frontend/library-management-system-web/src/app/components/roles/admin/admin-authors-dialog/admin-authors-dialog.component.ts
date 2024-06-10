import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { AuthorInsertDto } from '../../../../entities/dtos/library/author-insert-dto';
import { AuthorUpdateDto } from '../../../../entities/dtos/library/author-update-dto';
import { AuthorDto } from '../../../../entities/dtos/library/author-dto';
import { ControlStateMatcher } from '../../../../util/control-state-matcher';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { AuthorService } from '../../../../services/library/author.service';
import { ToastrService } from 'ngx-toastr';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../../../../entities/api/api-response';

@Component({
  selector: 'app-admin-authors-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatInputModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule, MatCheckboxModule],
  templateUrl: './admin-authors-dialog.component.html',
  styleUrl: './admin-authors-dialog.component.css'
})
export class AdminAuthorsDialogComponent {

  /* Referencia del formulario */
  authorForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()
  /* Dtos */
  authorInsertDto: AuthorInsertDto = { name: '', isFormerGraduated: false };
  authorUpdateDto: AuthorUpdateDto | undefined;
  authorDto: AuthorDto | undefined;

  constructor(
    public dialogRef: MatDialogRef<AdminAuthorsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private authorService: AuthorService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.authorForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(100)]],
      isFormerGraduated: [false]
    });
    
    // Obtener información cuando se use para editar
    if (dialogData.data) {
      // Obtener dto
      this.authorDto = dialogData.data as AuthorDto;
      // Setear la informacion en el formulario
      this.authorForm.patchValue({ name: this.authorDto.name, isFormerGraduated: this.authorDto.isFormerGraduated });
      // Inicializar el dto para actualizar
      this.authorUpdateDto = {
        authorId: this.authorDto.authorId,
        name: this.name?.value,
        isFormerGraduated: this.isFormerGraduated?.value
      }
    }
  }

  /* Getters */
  get name() {
    return this.authorForm.get('name');
  }

  get isFormerGraduated() {
    return this.authorForm.get('isFormerGraduated');
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
    // Obtener informacion de los campos
    this.authorInsertDto.name = this.name?.value
    this.authorInsertDto.isFormerGraduated = this.isFormerGraduated?.value
    // Realizar solicitud http
    this.authorService.create(this.authorInsertDto).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
          return;
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
    // Verificar que se inicializo el dto para actualizar
    if (!this.authorUpdateDto)
      return;
    // Obtener informacion de los campos
    this.authorUpdateDto.name = this.name?.value
    this.authorUpdateDto.isFormerGraduated = this.isFormerGraduated?.value
    // Realizar solicitud http
    this.authorService.update(this.authorUpdateDto).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
          return;
        }
        // Solicitud exitosa
        this.toastr.info(`${response.message}`, 'Exito', {
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

  /* */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }

}
