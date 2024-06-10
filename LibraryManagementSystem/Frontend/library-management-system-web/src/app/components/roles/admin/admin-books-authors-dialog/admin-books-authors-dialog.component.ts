import { Component, Inject } from '@angular/core';
import { ControlStateMatcher } from '../../../../util/control-state-matcher';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BookAuthorInsertDto } from '../../../../entities/dtos/library/book-author-insert-dto';
import { BookDto } from '../../../../entities/dtos/library/book-dto';
import { AuthorDto } from '../../../../entities/dtos/library/author-dto';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AuthorService } from '../../../../services/library/author.service';
import { BookAuthorService } from '../../../../services/library/book-author.service';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { ApiResponse } from '../../../../entities/api/api-response';

@Component({
  selector: 'app-admin-books-authors-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatInputModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './admin-books-authors-dialog.component.html',
  styleUrl: './admin-books-authors-dialog.component.css'
})
export class AdminBooksAuthorsDialogComponent {

  /* Referencia del formulario */
  bookAuthorsForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()

  bookAuthorInsertDto: BookAuthorInsertDto[] = []
  bookDto: BookDto | undefined;
  authors: AuthorDto[] | undefined;

  constructor(
    public dialogRef: MatDialogRef<AdminBooksAuthorsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private authorService: AuthorService,
    private bookAuthorService: BookAuthorService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.bookAuthorsForm = this.formBuilder.group({
      authorIds: ['', [Validators.required]]
    });

    /* Obtener autores */
    this.getAuthorsDto()

    // Cargar datos del dto para luego editarlo
    if (dialogData.data) {
      // Obtener dto
      this.bookDto = dialogData.data as BookDto;
      // Asignar valores en el formulario
      this.bookAuthorsForm.patchValue({
        authorIds: this.bookDto.authors?.map(a => a.authorId) // marcamos los autores del libro
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
    const bookId = this.bookDto?.bookId
    if (!bookId) {
      this.toastr.error(`Ocurrio un error al obtener Id del Libro`, 'Error', {
        timeOut: 5000,
        easeTime: 1000
      })
      return;
    }
    // obtener sub categorias seleccionadas
    const selectedSubCategories = this.authorIds?.value as number[];
    selectedSubCategories.forEach(aId => {
      this.bookAuthorInsertDto.push({ bookId: bookId, authorId: aId })
    })
    
    // Realizar solicitud http
    this.bookAuthorService.createMany(this.bookAuthorInsertDto).subscribe({
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
    const bookId = this.bookDto?.bookId
    if (!bookId) {
      this.toastr.error(`Ocurrio un error al obtener Id del Libro`, 'Error', {
        timeOut: 5000,
        easeTime: 1000
      })
      return;
    }
    // obtener sub categorias seleccionadas
    const selectedSubCategories = this.authorIds?.value as number[];
    selectedSubCategories.forEach(scId => {
      this.bookAuthorInsertDto.push({ bookId: bookId, authorId: scId })
    })

    // Realizar solicitud http
    this.bookAuthorService.updateMany(this.bookAuthorInsertDto).subscribe({
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
    return this.bookAuthorsForm.get('authorIds');
  }

  /* */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }

}
