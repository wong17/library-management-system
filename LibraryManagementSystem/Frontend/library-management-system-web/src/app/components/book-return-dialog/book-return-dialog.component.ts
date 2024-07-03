import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { BookLoanDto } from '../../entities/dtos/library/book-loan-dto';
import { UpdateReturnedBookLoanDto } from '../../entities/dtos/library/update-returned-book-loan-dto';
import { DialogData } from '../../util/dialog-data';
import { BookLoanService } from '../../services/library/book-loan.service';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../services/security/user.service';
import { ApiResponse } from '../../entities/api/api-response';

@Component({
  selector: 'app-book-return-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatIconModule, ReactiveFormsModule],
  templateUrl: './book-return-dialog.component.html',
  styleUrl: './book-return-dialog.component.css'
})
export class BookReturnDialogComponent {

  returnBookForm: FormGroup;

  updateReturnedBookLoanDto: UpdateReturnedBookLoanDto | undefined;
  bookLoanDto: BookLoanDto | undefined;

  constructor(
    public dialogRef: MatDialogRef<BookReturnDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private bookLoanService: BookLoanService,
    private toastr: ToastrService,
    private userService: UserService,
    private formBuilder: FormBuilder
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.returnBookForm = this.formBuilder.group({
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

    // Si se recibe la información de la solicitud del libro, llenar el dto para hacer la devolución de la solicitud
    if (dialogData.data) {
      this.bookLoanDto = dialogData.data as BookLoanDto;

      this.updateReturnedBookLoanDto = {
        bookLoanId: this.bookLoanDto.bookLoanId,
        returnedUserId: currentUserId as number
      }
    }
  }

  onSubmit() {
    // Verificar estado de la solicitud
    if (this.bookLoanDto?.state?.trimEnd() !== "PRESTADO") {
      this.toastr.warning(`No es posible devolver el libro si no esta PRESTADO`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }

    // Revisar si el dto se lleno correctamente
    if (!this.updateReturnedBookLoanDto || !this.updateReturnedBookLoanDto.returnedUserId) {
      this.toastr.warning(`No es posible devolver el libro, solicitud de préstamo o usuario no se encontró`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }

    // Realizar solicitud para devolver registro
    this.bookLoanService.updateReturnedBookLoan(this.updateReturnedBookLoanDto).subscribe({
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
        // Cerrar ventana de dialogo
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
