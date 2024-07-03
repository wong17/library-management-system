import { Component, Inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ApiResponse } from '../../entities/api/api-response';
import { BookLoanDto } from '../../entities/dtos/library/book-loan-dto';
import { UserService } from '../../services/security/user.service';
import { ToastrService } from 'ngx-toastr';
import { BookLoanService } from '../../services/library/book-loan.service';
import { DialogData } from '../../util/dialog-data';
import { UpdateBorrowedBookLoanDto } from '../../entities/dtos/library/update-borrowed-book-loan-dto';

@Component({
  selector: 'app-book-borrow-date-loan',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule, MatInputModule],
  templateUrl: './book-borrow-date-loan.component.html',
  styleUrl: './book-borrow-date-loan.component.css'
})
export class BookBorrowDateLoanComponent {

  /* Referencia del formulario */
  borrowDateForm: FormGroup;

  updateBorrowedBookLoanDto: UpdateBorrowedBookLoanDto | undefined;
  bookLoanDto: BookLoanDto | undefined;
  minDate: string | undefined;
  maxDate: string | undefined;

  /**
   * Inicializa el componente
   * @param dialogRef 
   * @param dialogData 
   * @param bookLoanService 
   * @param formBuilder 
   * @param toastr 
   * @param userService 
   */
  constructor(
    public dialogRef: MatDialogRef<BookBorrowDateLoanComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private bookLoanService: BookLoanService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private userService: UserService
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
      //
      this.closeDialog(true);
    }

    // 
    if (dialogData.data) {
      this.bookLoanDto = dialogData.data as BookLoanDto;

      this.updateBorrowedBookLoanDto = {
        bookLoanId: this.bookLoanDto.bookLoanId,
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

    if (this.bookLoanDto?.typeOfLoan?.trim() === 'DOMICILIO') {
      // Para tipo DOMICILIO: fechas desde hoy en adelante
      this.minDate = today.toISOString().substring(0, 16);
      this.maxDate = ''; // No límite superior específico
    } else if (this.bookLoanDto?.typeOfLoan?.trim() === 'SALA') {
      // Para tipo SALA: solo hoy entre las 7 AM y las 7 PM
      const startTime = new Date(today.setHours(7, 0, 0, 0)).toISOString().substring(0, 16);
      const endTime = new Date(today.setHours(19, 0, 0, 0)).toISOString().substring(0, 16);
      this.minDate = startTime;
      this.maxDate = endTime;
    }
  }

  /**
   * Valida que la fecha seleccionada sea válida según el tipo de préstamo
   * @param control - El control del formulario que contiene la fecha seleccionada
   * @returns Un objeto con la clave 'invalidDate' si la fecha es inválida, de lo contrario null
   */
  dateValidator(control: AbstractControl): { [key: string]: any } | null {
    // Convertir el valor del control a un objeto Date
    const borrowDate = new Date(control.value);
    const today = new Date();
    const endHour = 19; // Hora máxima permitida (7 PM)

    // La fecha de préstamo debe ser posterior a la fecha actual
    // La hora de préstamo no debe ser mayor a las 7 PM
    if (this.bookLoanDto?.typeOfLoan?.trim() === 'DOMICILIO') {
      if (borrowDate <= today || borrowDate.getHours() > endHour || (borrowDate.getHours() === endHour && borrowDate.getMinutes() > 0)) {
        return { 'invalidDate': true };
      }
    } else if (this.bookLoanDto?.typeOfLoan?.trim() === 'SALA') {
      const startHour = 7; // Hora mínima permitida (7 AM)
      const hour = borrowDate.getHours();
      const minutes = borrowDate.getMinutes();

      // La fecha de préstamo debe ser el mismo día
      // La hora de préstamo debe estar entre 7 AM y 7 PM
      // La hora de préstamo no debe ser posterior a las 7 PM ni anterior a las 7 AM
      if (borrowDate.toDateString() !== today.toDateString() || hour < startHour || (hour === endHour && minutes > 0) || hour > endHour) {
        return { 'invalidDate': true };
      }
    }
    return null;
  }

  /**
   * Se ejecuta al pulsar el boton de submit del formulario
   * @returns 
   */
  onSubmit() {
    // Verificar estado de la solicitud
    if (this.bookLoanDto?.state?.trimEnd() !== "CREADA") {
      this.toastr.warning(`No es posible prestar el libro sino se ha solicitado un préstamo`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }

    // Asignar fecha seleccionada a la solicitud de prestamo
    if (this.updateBorrowedBookLoanDto) {
      this.updateBorrowedBookLoanDto.dueDate = this.borrowDate?.value
    }

    // Verificar que la solicitud se haya iniciado correctamente desde el constructor y finalmente con la fecha de devolucion
    if (!this.updateBorrowedBookLoanDto || !this.updateBorrowedBookLoanDto.dueDate) {
      this.toastr.warning(`No es posible prestar el libro, solicitud de préstamo o fecha no se llenó correctamente`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }

    // Realizar solicitud para prestar registro
    this.bookLoanService.updateBorrowedBookLoan(this.updateBorrowedBookLoanDto).subscribe({
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

  /* */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }

  get borrowDate() {
    return this.borrowDateForm.get('borrowDate');
  }

}
