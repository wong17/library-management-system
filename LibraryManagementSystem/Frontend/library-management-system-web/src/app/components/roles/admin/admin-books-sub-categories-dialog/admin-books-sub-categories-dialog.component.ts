import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ControlStateMatcher } from '../../../../util/control-state-matcher';
import { BookSubCategoryInsertDto } from '../../../../entities/dtos/library/book-sub-category-insert-dto';
import { BookDto } from '../../../../entities/dtos/library/book-dto';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { SubCategoryService } from '../../../../services/library/sub-category.service';
import { ToastrService } from 'ngx-toastr';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { SubCategoryDto } from '../../../../entities/dtos/library/sub-category-dto';
import { ApiResponse } from '../../../../entities/api/api-response';
import { BookSubCategoryService } from '../../../../services/library/book-sub-category.service';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { CategoryDto } from '../../../../entities/dtos/library/category-dto';

@Component({
  selector: 'app-admin-books-sub-categories-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatInputModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './admin-books-sub-categories-dialog.component.html',
  styleUrl: './admin-books-sub-categories-dialog.component.css'
})
export class AdminBooksSubCategoriesDialogComponent {

  /* Referencia del formulario */
  bookSubCategoriesForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()

  bookSubCategoryInsertDto: BookSubCategoryInsertDto[] = []
  bookDto: BookDto | undefined;
  categoryDto: CategoryDto | null = { categoryId: 0, name: '' };
  subCategories: SubCategoryDto[] | undefined;
  filteredSubCategories: SubCategoryDto[] | undefined;

  constructor(
    public dialogRef: MatDialogRef<AdminBooksSubCategoriesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private subCategoryService: SubCategoryService,
    private bookSubCategoryService: BookSubCategoryService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.bookSubCategoriesForm = this.formBuilder.group({
      category: [''],
      subCategoryIds: ['', [Validators.required]]
    });

    /* Obtener categorias */
    this.getSubCategoriesDto()

    // Cargar datos del dto para luego editarlo
    if (dialogData.data) {
      // Obtener dto
      this.bookDto = dialogData.data as BookDto;
      this.categoryDto = this.bookDto.category
      // Asignar valores en el formulario
      this.bookSubCategoriesForm.patchValue({
        category: this.categoryDto?.name,
        subCategoryIds: this.bookDto.subCategories?.map(sc => sc.subCategoryId) // marcamos las sub categorias a las que pertenece el libro
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
    const selectedSubCategories = this.subCategoryIds?.value as number[];
    selectedSubCategories.forEach(scId => {
      this.bookSubCategoryInsertDto.push({ bookId: bookId, subCategoryId: scId })
    })
    
    // Realizar solicitud http
    this.bookSubCategoryService.createMany(this.bookSubCategoryInsertDto).subscribe({
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
    const selectedSubCategories = this.subCategoryIds?.value as number[];
    selectedSubCategories.forEach(scId => {
      this.bookSubCategoryInsertDto.push({ bookId: bookId, subCategoryId: scId })
    })

    // Realizar solicitud http
    this.bookSubCategoryService.updateMany(this.bookSubCategoryInsertDto).subscribe({
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

  private getSubCategoriesDto(): void {
    this.subCategoryService.getAll().subscribe({
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
        this.subCategories = list as SubCategoryDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      },
      complete: () => {
        // Filtramos sub categorias una vez las obtengamos
        this.filteredSubCategories = this.subCategories?.filter(subCategory => subCategory.category?.categoryId === this.categoryDto?.categoryId); 
      }
    })
  }

  /* Getters */
  get category() {
    return this.bookSubCategoriesForm.get('category');
  }

  get subCategoryIds() {
    return this.bookSubCategoriesForm.get('subCategoryIds');
  }

  /* */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }

}
