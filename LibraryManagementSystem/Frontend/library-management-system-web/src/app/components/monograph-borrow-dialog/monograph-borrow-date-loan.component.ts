import { Component, Inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { UpdateBorrowedMonographLoanDto } from '../../entities/dtos/library/update-borrowed-monograph-loan-dto';
import { MonographLoanDto } from '../../entities/dtos/library/monograph-loan-dto';
import { MonographLoanService } from '../../services/library/monograph-loan.service';
import { DialogData } from '../../util/dialog-data';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../services/security/user.service';
import { ApiResponse } from '../../entities/api/api-response';

@Component({
  selector: 'app-monograph-borrow-date-loan',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatIconModule, ReactiveFormsModule],
  templateUrl: './monograph-borrow-date-loan.component.html',
  styleUrl: './monograph-borrow-date-loan.component.css'
})
export class MonographBorrowDateLoanComponent {

  borrowDateForm: FormGroup;

  updateBorrowedMonographLoanDto: UpdateBorrowedMonographLoanDto | undefined;
  monographLoanDto: MonographLoanDto | undefined;
  minDate: string | undefined;
  maxDate: string | undefined;

  /**
   * Inicializa el componente
   * @param dialogRef 
   * @param dialogData 
   * @param monographLoanService 
   * @param toastr 
   * @param userService 
   * @param formBuilder 
   */
  constructor(
    public dialogRef: MatDialogRef<MonographBorrowDateLoanComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private monographLoanService: MonographLoanService,
    private toastr: ToastrService,
    private userService: UserService,
    private formBuilder: FormBuilder
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.borrowDateForm = this.formBuilder.group({
      borrowDate: ['', [Validators.required, this.dateValidator.bind(this)]]
    });

    // Verificar que hayan iniciado sesion
    const currentUserId = userService.currentUser?.userId;
    if (!currentUserId) {
      this.toastr.warning(`No es posible préstar el libro sino se ha iniciado sesión`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      // Cerrar la ventana de dialogo
      this.closeDialog(true);
    }

    // Si se recibe la información de la solicitud del libro, llenar el dto para hacer el prestamo 
    if (dialogData.data) {
      this.monographLoanDto = dialogData.data as MonographLoanDto;

      this.updateBorrowedMonographLoanDto = {
        monographLoanId: this.monographLoanDto.monographLoanId,
        borrowedUserId: currentUserId as number,
        dueDate: new Date() // default
      }
    }

    // Establecer restricciones de fecha y hora según el tipo de préstamo
    this.setDateRestrictions();
  }

  /**
   * Establece restricciones de fecha y hora según el tipo de préstamo
   */
  setDateRestrictions() {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    
    const minHour = '07:00';
    const maxHour = '19:00';
    
    this.minDate = `${year}-${month}-${day}T${minHour}`;
    this.maxDate = `${year}-${month}-${day}T${maxHour}`;
  }

  /**
   * Valida que la fecha seleccionada sea válida 
   * @param control - El control del formulario que contiene la fecha seleccionada
   * @returns Un objeto con la clave 'invalidDate' si la fecha es inválida, de lo contrario null
   */
  dateValidator(control: AbstractControl): { [key: string]: any } | null {
    const borrowDate = new Date(control.value);
    const today = new Date();
    const startHour = 7; // Hora mínima permitida (7 AM)
    const endHour = 19; // Hora máxima permitida (7 PM)
    const hour = borrowDate.getHours();
    const minutes = borrowDate.getMinutes();

    today.setHours(0, 0, 0, 0); // Resetear la hora para asegurar que es hoy

    if (borrowDate.toDateString() !== today.toDateString() || hour < startHour || (hour === endHour && minutes > 0) || hour > endHour) {
      return { 'invalidDate': true };
    }
    return null;
  }

  /**
   * Se ejecuta al pulsar el botón submit del formulario
   * @returns 
   */
  onSubmit() {
    // Validar estado de la solicitud
    if (this.monographLoanDto?.state?.trimEnd() !== "CREADA") {
      this.toastr.warning(`No es posible préstar la monografía sino se ha solicitado un préstamo`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }

    // Asignar fecha seleccionada a la solicitud de prestamo
    if (this.updateBorrowedMonographLoanDto) {
      this.updateBorrowedMonographLoanDto.dueDate = this.borrowDate?.value
    }

    // Verificar que la solicitud se haya iniciado correctamente desde el constructor y finalmente con la fecha de devolucion
    if (!this.updateBorrowedMonographLoanDto || !this.updateBorrowedMonographLoanDto.dueDate) {
      this.toastr.warning(`No es posible prestar el libro, solicitud de préstamo, fecha, usuario o monografía no se encontró`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }

    // Realizar solicitud para prestar registro
    this.monographLoanService.updateBorrowedMonographLoan(this.updateBorrowedMonographLoanDto).subscribe({
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
        // Cerra la ventana de dialogo
        this.closeDialog(true)
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      }
    })
  }

  /**
   * Cierra la ventana de dialogo
   * @param done 
   */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }

  /* */
  get borrowDate() {
    return this.borrowDateForm.get('borrowDate');
  }

}
