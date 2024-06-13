import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ControlStateMatcher } from '../../../../util/control-state-matcher';
import { CategoryInsertDto } from '../../../../entities/dtos/library/category-insert-dto';
import { CategoryUpdateDto } from '../../../../entities/dtos/library/category-update-dto';
import { CategoryDto } from '../../../../entities/dtos/library/category-dto';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CategoryService } from '../../../../services/library/category.service';
import { ToastrService } from 'ngx-toastr';
import { ApiResponse } from '../../../../entities/api/api-response';

@Component({
  selector: 'app-admin-categories-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatInputModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule],
  templateUrl: './admin-categories-dialog.component.html',
  styleUrl: './admin-categories-dialog.component.css'
})
export class AdminCategoriesDialogComponent {

  /* Referencia del formulario */
  categoryForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()
  /* Dtos */
  categoryInsertDto: CategoryInsertDto = { name: '' };
  categoryUpdateDto: CategoryUpdateDto | undefined;
  categoryDto: CategoryDto | undefined;

  constructor(
    public dialogRef: MatDialogRef<AdminCategoriesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private categoryService: CategoryService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.categoryForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(100)]]
    });
    // Obtener información cuando se use para editar
    if (dialogData.data) {
      // Obtener dto
      this.categoryDto = dialogData.data as CategoryDto;
      // Setear la informacion en el formulario
      this.categoryForm.patchValue({ name: this.categoryDto.name });
      // Inicializar el dto para actualizar
      this.categoryUpdateDto = {
        categoryId: this.categoryDto.categoryId,
        name: this.name?.value
      }
    }
  }

  /* Getters */
  get name() {
    return this.categoryForm.get('name');
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
    this.categoryInsertDto.name = this.name?.value
    // Realizar solicitud http
    this.categoryService.create(this.categoryInsertDto).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 5000
          })
          return;
        }
        // Solicitud exitosa
        this.toastr.success(`${response.message}`, 'Exito', {
          timeOut: 5000
        })

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
    if (!this.categoryUpdateDto)
      return;
    // Obtener informacion de los campos
    this.categoryUpdateDto.name = this.name?.value
    // Realizar solicitud http
    this.categoryService.update(this.categoryUpdateDto).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 5000
          })
          return;
        }
        // Solicitud exitosa
        this.toastr.info(`${response.message}`, 'Exito', {
          timeOut: 5000
        })

        this.closeDialog(true);
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  /* */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }
}
