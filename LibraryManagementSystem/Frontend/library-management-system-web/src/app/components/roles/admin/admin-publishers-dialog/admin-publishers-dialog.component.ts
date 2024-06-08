import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { PublisherService } from '../../../../services/library/publisher.service';
import { PublisherInsertDto } from '../../../../entities/dtos/library/publisher-insert-dto';
import { PublisherDto } from '../../../../entities/dtos/library/publisher-dto';
import { PublisherUpdateDto } from '../../../../entities/dtos/library/publisher-update-dto';
import { ControlStateMatcher } from '../../../../util/control-state-matcher';

@Component({
  selector: 'app-admin-publishers-dialog',
  standalone: true,
  imports: [MatDialogTitle, MatDialogContent, MatDialogActions, MatButtonModule, MatDialogModule, MatInputModule, MatIconModule, ReactiveFormsModule, MatFormFieldModule, ToastrModule],
  templateUrl: './admin-publishers-dialog.component.html',
  styleUrl: './admin-publishers-dialog.component.css'
})
export class AdminPublishersDialogComponent {

  /* Referencia del formulario */
  publisherForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()
  /* Dtos */
  publisherInsertDto: PublisherInsertDto = { name: '' };
  publisherUpdateDto: PublisherUpdateDto | undefined;
  publisherDto: PublisherDto | undefined;

  constructor(
    public dialogRef: MatDialogRef<AdminPublishersDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private publisherService: PublisherService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.publisherForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(100)]]
    });
    
    // Obtener información cuando se use para editar
    if (dialogData.data) {
      // Obtener dto
      this.publisherDto = dialogData.data as PublisherDto;
      // Setear la informacion en el formulario
      this.publisherForm.patchValue({ name: this.publisherDto.name });
      // Inicializar el dto para actualizar
      this.publisherUpdateDto = {
        publisherId: this.publisherDto.publisherId,
        name: this.name?.value
      }
    }
  }

  /* Getters */
  get name() {
    return this.publisherForm.get('name');
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
    this.publisherInsertDto.name = this.name?.value
    // Realizar solicitud http
    this.publisherService.create(this.publisherInsertDto).subscribe({
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
        this.toastr.error(error.message, 'Error', {
          timeOut: 3000,
          easeTime: 1000
        });
      }
    })
  }

  /* Actualizar */
  private update(): void {
    // Verificar que se inicializo el dto para actualizar
    if (!this.publisherUpdateDto)
      return;
    // Obtener informacion de los campos
    this.publisherUpdateDto.name = this.name?.value
    // Realizar solicitud http
    this.publisherService.update(this.publisherUpdateDto).subscribe({
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
        this.toastr.error(error.message, 'Error', {
          timeOut: 3000,
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
