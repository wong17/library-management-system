import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ControlStateMatcher } from '../../../../util/control-state-matcher';
import { SubCategoryDto } from '../../../../entities/dtos/library/sub-category-dto';
import { SubCategoryUpdateDto } from '../../../../entities/dtos/library/sub-category-update-dto';
import { SubCategoryInsertDto } from '../../../../entities/dtos/library/sub-category-insert-dto';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { SubCategoryService } from '../../../../services/library/sub-category.service';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../../../../entities/api/api-response';
import { CategoryService } from '../../../../services/library/category.service';
import { CategoryDto } from '../../../../entities/dtos/library/category-dto';

@Component({
  selector: 'app-admin-sub-categories-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatInputModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './admin-sub-categories-dialog.component.html',
  styleUrl: './admin-sub-categories-dialog.component.css'
})
export class AdminSubCategoriesDialogComponent {

  /* Referencia del formulario */
  subCategoryForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()
  /* Dtos */
  subCategoryInsertDto: SubCategoryInsertDto = { name: '', categoryId: 0 };
  subCategoryUpdateDto: SubCategoryUpdateDto | undefined;
  subCategoryDto: SubCategoryDto | undefined;
  /* Categorias */
  categories: CategoryDto[] | undefined;

  constructor(
    public dialogRef: MatDialogRef<AdminSubCategoriesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private subCategoryService: SubCategoryService,
    private categoryService: CategoryService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.subCategoryForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(100)]],
      categoryId: ['', [Validators.required]]
    });

    /* Obtener categorias */
    this.getCategoriesDto();

    // Obtener información cuando se use para editar
    if (dialogData.data) {
      // Obtener dto
      this.subCategoryDto = dialogData.data as SubCategoryDto;
      // Setear la informacion en el formulario
      this.subCategoryForm.patchValue({ name: this.subCategoryDto.name, categoryId: this.subCategoryDto.category?.categoryId });
      // Inicializar el dto para actualizar
      this.subCategoryUpdateDto = {
        subCategoryId: this.subCategoryDto.subCategoryId,
        name: this.name?.value,
        categoryId: this.categoryId?.value
      }
    }
  }

  /* Getters */
  get name() {
    return this.subCategoryForm.get('name');
  }

  get categoryId() {
    return this.subCategoryForm.get('categoryId');
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
    this.subCategoryInsertDto.name = this.name?.value
    this.subCategoryInsertDto.categoryId = this.categoryId?.value
    // Realizar solicitud http
    this.subCategoryService.create(this.subCategoryInsertDto).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 3000,
            easeTime: 1000
          })
          return;
        }
        // Solicitud exitosa
        this.toastr.success(`${response.message}`, 'Exito', {
          timeOut: 3000,
          easeTime: 1000
        })

        this.closeDialog(true);
      },
      error: error => {
        if (error instanceof HttpErrorResponse) {
          //
          const response = error.error as ApiResponse;
          // BadRequest
          if (response.isSuccess === 1 && response.statusCode === 400) {
            this.toastr.warning(`${response.message}`, 'Atención', {
              timeOut: 3000,
              easeTime: 1000
            });
            return;
          }
          // InternalServerError
          if (response.isSuccess === 3 && response.statusCode === 500) {
            this.toastr.error(`${response.message}`, 'Error', {
              timeOut: 3000,
              easeTime: 1000
            });
          }
        }
      }
    })
  }

  /* Actualizar */
  private update(): void {
    // Verificar que se inicializo el dto para actualizar
    if (!this.subCategoryUpdateDto)
      return;
    // Obtener informacion de los campos
    this.subCategoryUpdateDto.name = this.name?.value
    this.subCategoryUpdateDto.categoryId = this.categoryId?.value
    // Realizar solicitud http
    this.subCategoryService.update(this.subCategoryUpdateDto).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 3000,
            easeTime: 1000
          })
          return;
        }
        // Solicitud exitosa
        this.toastr.info(`${response.message}`, 'Exito', {
          timeOut: 3000,
          easeTime: 1000
        })

        this.closeDialog(true);
      },
      error: error => {
        if (error instanceof HttpErrorResponse) {
          //
          const response = error.error as ApiResponse;
          // BadRequest
          if (response.isSuccess === 1 && response.statusCode === 400) {
            this.toastr.warning(`${response.message}`, 'Atención', {
              timeOut: 3000,
              easeTime: 1000
            });
            return;
          }
          // InternalServerError
          if (response.isSuccess === 3 && response.statusCode === 500) {
            this.toastr.error(`${response.message}`, 'Error', {
              timeOut: 3000,
              easeTime: 1000
            });
          }
        }
      }
    })
  }

  private getCategoriesDto(): void {
    this.categoryService.getAll().subscribe({
      next: response => {
        // Verificar si la respuesta es nula
        if (!response) {
          this.toastr.error('No se recibió respuesta del servidor', 'Error', {
            timeOut: 3000,
            easeTime: 1000
          })
          return;
        }
        // Verificar si ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 3000,
            easeTime: 1000
          })
          return;
        }
        // Verificar si el resultado es un array válido
        const list = response.result;
        if (!Array.isArray(list)) {
          this.toastr.error(`El resultado no es un array válido: ${response.message}`, 'Error', {
            timeOut: 3000,
            easeTime: 1000
          })
          return;
        }
        // Asignar datos
        this.categories = list as CategoryDto[];
      },
      error: error => {
        if (error instanceof HttpErrorResponse) {
          //
          const response = error.error as ApiResponse;
          // BadRequest
          if (response.isSuccess === 1 && response.statusCode === 400) {
            this.toastr.warning(`${response.message}`, 'Atención', {
              timeOut: 3000,
              easeTime: 1000
            });
            return;
          }
          // InternalServerError
          if (response.isSuccess === 3 && response.statusCode === 500) {
            this.toastr.error(`${response.message}`, 'Error', {
              timeOut: 3000,
              easeTime: 1000
            });
          }
        }
      }
    })
  }

  /* */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }
}
