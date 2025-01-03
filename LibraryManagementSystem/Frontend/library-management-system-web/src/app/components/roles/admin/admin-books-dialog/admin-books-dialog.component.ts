import { Component, Inject } from '@angular/core';
import { ControlStateMatcher } from '../../../../util/control-state-matcher';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BookInsertDto } from '../../../../entities/dtos/library/book-insert-dto';
import { BookUpdateDto } from '../../../../entities/dtos/library/book-update-dto';
import { BookDto } from '../../../../entities/dtos/library/book-dto';
import { CategoryDto } from '../../../../entities/dtos/library/category-dto';
import { PublisherDto } from '../../../../entities/dtos/library/publisher-dto';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { CategoryService } from '../../../../services/library/category.service';
import { ToastrService } from 'ngx-toastr';
import { PublisherService } from '../../../../services/library/publisher.service';
import { BookService } from '../../../../services/library/book.service';
import { ApiResponse } from '../../../../entities/api/api-response';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-books-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatInputModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule, MatSelectModule, MatCheckboxModule, CommonModule],
  templateUrl: './admin-books-dialog.component.html',
  styleUrl: './admin-books-dialog.component.css'
})
export class AdminBooksDialogComponent {

  /* Referencia del formulario */
  bookForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()
  /* Dtos */
  bookInsertDto: BookInsertDto = {
    isbN10: '', isbN13: '', title: '', classification: '', description: '',
    isActive: false, categoryId: 0, image: null, numberOfCopies: 0, publisherId: 0, publicationYear: 0
  };
  bookUpdateDto: BookUpdateDto | undefined;
  bookDto: BookDto | undefined;
  /* Categorias */
  categories: CategoryDto[] | undefined;
  /* Editoriales */
  publishers: PublisherDto[] | undefined;
  /* */
  selectedFile: File | null = null;
  imagePreview: string | null = null;
  /* Año actual */
  currentYear: number = new Date().getFullYear();

  constructor(
    public dialogRef: MatDialogRef<AdminBooksDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private bookService: BookService,
    private categoryService: CategoryService,
    private publisherService: PublisherService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.bookForm = this.formBuilder.group({
      isbn10: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(13)]],
      isbn13: ['', [Validators.required, Validators.minLength(14), Validators.maxLength(17)]],
      classification: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(25)]],
      title: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(100)]],
      description: ['', [Validators.maxLength(500)]],
      publicationYear: ['', [Validators.required]],
      image: [''],
      publisherId: ['', [Validators.required]],
      categoryId: ['', [Validators.required]],
      numberOfCopies: ['', [Validators.required]],
      isActive: ['', [Validators.required]]
    });

    // Cargar datos
    this.loadData();

    //Obtener información cuando se use para editar
    if (dialogData.data) {
      // Obtener dto
      this.bookDto = dialogData.data as BookDto;
      // Verificar si tiene una imagen asociada
      let imageStr: string | null = this.bookDto.image
      if (imageStr) {
        // Agregar prefijo 'data:image/*;base64,'
        const base64Data = imageStr
        const base64String = `data:image/*;base64,${base64Data}`
        imageStr = base64String
      }
      // Setear la informacion en el formulario
      this.bookForm.patchValue({
        isbn10: this.bookDto.isbN10, 
        isbn13: this.bookDto.isbN13,
        classification: this.bookDto.classification, 
        title: this.bookDto.title,
        description: this.bookDto.description, 
        publicationYear: this.bookDto.publicationYear,
        image: imageStr, 
        publisherId: this.bookDto.publisher?.publisherId,
        categoryId: this.bookDto.category?.categoryId, 
        numberOfCopies: this.bookDto.numberOfCopies,
        isActive: this.bookDto.isActive
      });

      // Inicializar el dto para actualizar
      this.bookUpdateDto = {
        bookId: this.bookDto.bookId,
        isbN10: this.isbn10?.value,
        isbN13: this.isbn13?.value,
        classification: this.classification?.value,
        title: this.title?.value,
        description: this.description?.value,
        publicationYear: this.publicationYear?.value,
        image: this.image?.value,
        publisherId: this.publisherId?.value,
        categoryId: this.categoryId?.value,
        numberOfCopies: this.numberOfCopies?.value,
        isActive: this.isActive?.value
      }
    }
  }

  /* Para seleccionar una imagen */
  onFileSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      this.selectedFile = fileInput.files[0];

      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result as string;
        this.image?.setValue(this.imagePreview);
      };
      reader.readAsDataURL(this.selectedFile);
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
    // Obtener informacion de los campos
    this.bookInsertDto.isbN10 = this.isbn10?.value
    this.bookInsertDto.isbN13 = this.isbn13?.value
    this.bookInsertDto.classification = this.classification?.value
    this.bookInsertDto.title = this.title?.value
    this.bookInsertDto.description = this.description?.value
    this.bookInsertDto.publicationYear = this.publicationYear?.value

    // Si se selecciono una imagen
    if (this.image?.value) {
      const base64String = this.image?.value
      // Elimina el prefijo 'data:image/*;base64,'
      const base64Data = base64String.split(',')[1];
      this.bookInsertDto.image = base64Data
    } else {
      this.bookInsertDto.image = this.image?.value
    }

    this.bookInsertDto.publisherId = this.publisherId?.value
    this.bookInsertDto.categoryId = this.categoryId?.value
    this.bookInsertDto.numberOfCopies = this.numberOfCopies?.value
    this.bookInsertDto.isActive = this.isActive?.value

    // Realizar solicitud http
    this.bookService.create(this.bookInsertDto).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`${response.message}`, 'Error', {
            timeOut: 5000
          })
        }
        // Solicitud exitosa
        this.toastr.success(`${response.message}`, 'Exito', {
          timeOut: 5000
        })
        //
        this.closeDialog(true);
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  /* Actualizar */
  private update(): void {
    // Verificar que se inicializo el dto para actualizar
    if (!this.bookUpdateDto)
      return;
    // Obtener informacion de los campos
    this.bookUpdateDto.isbN10 = this.isbn10?.value
    this.bookUpdateDto.isbN13 = this.isbn13?.value
    this.bookUpdateDto.classification = this.classification?.value
    this.bookUpdateDto.title = this.title?.value
    this.bookUpdateDto.description = this.description?.value
    this.bookUpdateDto.publicationYear = this.publicationYear?.value

    // Si se selecciono una imagen
    if (this.image?.value) {
      const base64String = this.image?.value
      // Elimina el prefijo 'data:image/*;base64,'
      const base64Data = base64String.split(',')[1];
      this.bookUpdateDto.image = base64Data
    } else {
      this.bookUpdateDto.image = this.image?.value
    }

    this.bookUpdateDto.publisherId = this.publisherId?.value
    this.bookUpdateDto.categoryId = this.categoryId?.value
    this.bookUpdateDto.numberOfCopies = this.numberOfCopies?.value
    this.bookUpdateDto.isActive = this.isActive?.value

    // Realizar solicitud http
    this.bookService.update(this.bookUpdateDto).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`${response.message}`, 'Error', {
            timeOut: 5000
          })
          return;
        }
        // Solicitud exitosa
        this.toastr.info(`${response.message}`, 'Exito', {
          timeOut: 5000
        })
        // 
        this.closeDialog(true)
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  private async loadData(): Promise<void> {
    await Promise.all([
      this.getCategoriesDto(),
      this.getPublishersDto()
    ]);
  }

  private getCategoriesDto(): void {
    this.categoryService.getAll().subscribe({
      next: response => {
        // Verificar si la respuesta es nula
        if (!response) {
          this.toastr.error('No se recibió respuesta del servidor', 'Error', {
            timeOut: 5000
          })
          return;
        }
        // Verificar si ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`${response.message}`, 'Error', {
            timeOut: 5000
          })
          return;
        }
        // Verificar si el resultado es un array válido
        const list = response.result;
        if (!Array.isArray(list)) {
          this.toastr.error(`El resultado no es un array válido: ${response.message}`, 'Error', {
            timeOut: 5000
          })
          return;
        }
        // Asignar datos
        this.categories = list as CategoryDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  private getPublishersDto(): void {
    this.publisherService.getAll().subscribe({
      next: response => {
        // Verificar si la respuesta es nula
        if (!response) {
          this.toastr.error('No se recibió respuesta del servidor', 'Error', {
            timeOut: 5000
          })
          return;
        }
        // Verificar si ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`${response.message}`, 'Error', {
            timeOut: 5000
          })
          return;
        }
        // Verificar si el resultado es un array válido
        const list = response.result;
        if (!Array.isArray(list)) {
          this.toastr.error(`El resultado no es un array válido: ${response.message}`, 'Error', {
            timeOut: 5000
          })
          return;
        }
        // Asignar datos
        this.publishers = list as PublisherDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  /* Getters */
  get isbn10() {
    return this.bookForm.get('isbn10');
  }

  get isbn13() {
    return this.bookForm.get('isbn13');
  }

  get classification() {
    return this.bookForm.get('classification');
  }

  get title() {
    return this.bookForm.get('title');
  }

  get description() {
    return this.bookForm.get('description');
  }

  get image() {
    return this.bookForm.get('image');
  }

  get publicationYear() {
    return this.bookForm.get('publicationYear');
  }

  get publisherId() {
    return this.bookForm.get('publisherId');
  }

  get categoryId() {
    return this.bookForm.get('categoryId');
  }

  get subCategoryIds() {
    return this.bookForm.get('subCategoryIds');
  }

  get authorIds() {
    return this.bookForm.get('authorIds');
  }

  get numberOfCopies() {
    return this.bookForm.get('numberOfCopies');
  }

  get isActive() {
    return this.bookForm.get('isActive');
  }

  /* */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }

}
