import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { UpdateReturnedMonographLoanDto } from '../../entities/dtos/library/update-returned-monograph-loan-dto';
import { MonographLoanDto } from '../../entities/dtos/library/monograph-loan-dto';
import { DialogData } from '../../util/dialog-data';
import { MonographLoanService } from '../../services/library/monograph-loan.service';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../../services/security/user.service';
import { ApiResponse } from '../../entities/api/api-response';

@Component({
  selector: 'app-monograph-return-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatIconModule, ReactiveFormsModule],
  templateUrl: './monograph-return-dialog.component.html',
  styleUrl: './monograph-return-dialog.component.css'
})
export class MonographReturnDialogComponent {

  returnMonographForm: FormGroup;

  updateReturnedMonographLoanDto: UpdateReturnedMonographLoanDto | undefined;
  monographLoanDto: MonographLoanDto | undefined;

  constructor(
    public dialogRef: MatDialogRef<MonographReturnDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dialogData: DialogData,
    private monographLoanService: MonographLoanService,
    private toastr: ToastrService,
    private userService: UserService,
    private formBuilder: FormBuilder
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.returnMonographForm = this.formBuilder.group({
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
      this.monographLoanDto = dialogData.data as MonographLoanDto;

      this.updateReturnedMonographLoanDto = {
        monographLoanId: this.monographLoanDto.monographLoanId,
        returnedUserId: currentUserId as number
      }
    }
  }

  onSubmit() {
    // Validar estado de la solicitud
    if (this.monographLoanDto?.state?.trimEnd() !== "PRESTADA") {
      this.toastr.warning(`No es posible devolver la monografía si no esta PRESTADA`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }

    // Revisar si el dto se lleno correctamente
    if (!this.updateReturnedMonographLoanDto || !this.updateReturnedMonographLoanDto.returnedUserId
      || !this.updateReturnedMonographLoanDto.monographLoanId
    ) {
      this.toastr.warning(`No es posible devolver la monografía, solicitud de préstamo, usuario o monografía no se encontró`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }

    // Realizar solicitud para devolver registro
    this.monographLoanService.updateReturnedMonographLoan(this.updateReturnedMonographLoanDto).subscribe({
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
        // Cerrar la ventana de dialogo
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

  /* */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }
}
