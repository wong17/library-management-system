import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ControlStateMatcher } from '../../../../util/control-state-matcher';
import { MonographInsertDto } from '../../../../entities/dtos/library/monograph-insert-dto';
import { MonographDto } from '../../../../entities/dtos/library/monograph-dto';
import { MonographUpdateDto } from '../../../../entities/dtos/library/monograph-update-dto';
import { MonographAuthorInsertDto } from '../../../../entities/dtos/library/monograph-author-insert-dto';
import { CareerDto } from '../../../../entities/dtos/university/career-dto';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { MonographService } from '../../../../services/library/monograph.service';
import { ToastrService } from 'ngx-toastr';
import { CareerService } from '../../../../services/university/career.service';
import { ApiResponse } from '../../../../entities/api/api-response';

@Component({
  selector: 'app-admin-monographs-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatInputModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule, MatSelectModule, MatCheckboxModule, CommonModule],
  templateUrl: './admin-monographs-dialog.component.html',
  styleUrl: './admin-monographs-dialog.component.css'
})
export class AdminMonographsDialogComponent {

  /* Referencia del formulario */
  monographForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()
  /* Dtos */
  monographInsertDto: MonographInsertDto = {
    title: '', description: '', classification: '', image: null, presentationDate: new Date(), careerId: 0, tutor: '', isActive: false
  };
  monographAuthorInsertDto: MonographAuthorInsertDto[] = []
  monographUpdateDto: MonographUpdateDto | undefined;
  monographDto: MonographDto | undefined;
  /* Carreras */
  careers: CareerDto[] | undefined;
  /* */
  selectedFile: File | null = null;
  imagePreview: string | null = null;

  constructor(
    public dialogRef: MatDialogRef<AdminMonographsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private monographService: MonographService,
    private careerService: CareerService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.monographForm = this.formBuilder.group({
      title: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(250)]],
      description: ['', [Validators.maxLength(500)]],
      classification: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(25)]],
      presentationDate: ['', [Validators.required]],
      careerId: ['', [Validators.required]],
      tutor: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(100)]],
      image: [''],
      isActive: ['', [Validators.required]]
    });

    // Cargar datos
    this.getCareersDto();

    //Obtener información cuando se use para editar
    if (dialogData.data) {
      // Obtener dto
      this.monographDto = dialogData.data as MonographDto;
      // Verificar si tiene una imagen asociada
      let imageStr: string | null = this.monographDto.image
      if (imageStr) {
        // Agregar prefijo 'data:image/*;base64,'
        const base64Data = imageStr
        const base64String = `data:image/*;base64,${base64Data}`
        imageStr = base64String
      }
      // Setear la informacion en el formulario
      this.monographForm.patchValue({
        title: this.monographDto.title, description: this.monographDto.description,
        classification: this.monographDto.classification, presentationDate: this.monographDto.presentationDate,
        careerId: this.monographDto.career?.careerId, tutor: this.monographDto.tutor,
        image: imageStr, isActive: this.monographDto.isActive
      });

      // Inicializar el dto para actualizar
      this.monographUpdateDto = {
        monographId: this.monographDto.monographId,
        title: this.title?.value,
        description: this.description?.value,
        classification: this.classification?.value,
        presentationDate: this.presentationDate?.value,
        image: this.image?.value,
        tutor: this.tutor?.value,
        isActive: this.isActive?.value,
        careerId: this.careerId?.value
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
    this.monographInsertDto.title = this.title?.value
    this.monographInsertDto.classification = this.classification?.value
    this.monographInsertDto.description = this.description?.value
    this.monographInsertDto.tutor = this.tutor?.value
    this.monographInsertDto.presentationDate = this.presentationDate?.value
    this.monographInsertDto.careerId = this.careerId?.value
    this.monographInsertDto.isActive = this.isActive?.value

    // Si se selecciono una imagen
    if (this.image?.value) {
      const base64String = this.image?.value
      // Elimina el prefijo 'data:image/*;base64,'
      const base64Data = base64String.split(',')[1];
      this.monographInsertDto.image = base64Data
    } else {
      this.monographInsertDto.image = this.image?.value
    }

    // Realizar solicitud http
    this.monographService.create(this.monographInsertDto).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
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
    if (!this.monographUpdateDto)
      return;
    // Obtener informacion de los campos
    this.monographUpdateDto.title = this.title?.value
    this.monographUpdateDto.classification = this.classification?.value
    this.monographUpdateDto.description = this.description?.value
    this.monographUpdateDto.tutor = this.tutor?.value
    this.monographUpdateDto.presentationDate = this.presentationDate?.value
    this.monographUpdateDto.careerId = this.careerId?.value
    this.monographUpdateDto.isActive = this.isActive?.value

    // Si se selecciono una imagen
    if (this.image?.value) {
      const base64String = this.image?.value
      // Elimina el prefijo 'data:image/*;base64,'
      const base64Data = base64String.split(',')[1];
      this.monographUpdateDto.image = base64Data
    } else {
      this.monographUpdateDto.image = this.image?.value
    }

    // Realizar solicitud http
    this.monographService.update(this.monographUpdateDto).subscribe({
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

  private getCareersDto(): void {
    this.careerService.getAll().subscribe({
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
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
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
        this.careers = list as CareerDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  /* Getters */
  get title() {
    return this.monographForm.get('title');
  }

  get description() {
    return this.monographForm.get('description');
  }

  get classification() {
    return this.monographForm.get('classification');
  }

  get presentationDate() {
    return this.monographForm.get('presentationDate');
  }

  get image() {
    return this.monographForm.get('image');
  }

  get tutor() {
    return this.monographForm.get('tutor');
  }

  get isActive() {
    return this.monographForm.get('isActive');
  }

  get careerId() {
    return this.monographForm.get('careerId');
  }

  /* */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }

}
