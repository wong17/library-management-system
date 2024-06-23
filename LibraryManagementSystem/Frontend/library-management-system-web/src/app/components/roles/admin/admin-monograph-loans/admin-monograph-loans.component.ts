import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog'
import { MonographLoanDto } from '../../../../entities/dtos/library/monograph-loan-dto';
import { MonographLoanService } from '../../../../services/library/monograph-loan.service';
import { ToastrService } from 'ngx-toastr';
import { ApiResponse } from '../../../../entities/api/api-response';
import { DeleteDialogComponent } from '../../../delete-dialog/delete-dialog.component';
import { MonographLoanSignalRService } from '../../../../services/signalr-hubs/monograph-loan-signal-r.service';
import { UpdateBorrowedMonographLoanDto } from '../../../../entities/dtos/library/update-borrowed-monograph-loan-dto';
import { UpdateReturnedMonographLoanDto } from '../../../../entities/dtos/library/update-returned-monograph-loan-dto';

@Component({
  selector: 'app-admin-monograph-loans',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule],
  templateUrl: './admin-monograph-loans.component.html',
  styleUrl: './admin-monograph-loans.component.css'
})
export class AdminMonographLoansComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['monographLoanId', 'state', 'loanDate', 'dueDate', 'returnDate', 'student', 'monograph', 'options'];

  /*  */
  dataSource: MatTableDataSource<MonographLoanDto> = new MatTableDataSource<MonographLoanDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;

  constructor(private monographLoanService: MonographLoanService, private toastr: ToastrService, private dialog: MatDialog, private mlSignalR: MonographLoanSignalRService) { }

  ngOnInit(): void {
    this.getMonographLoansDto();
    // Conectarse al Hub de monografias
    this.mlSignalR.monographLoanNotification.subscribe((loanCreated: boolean) => {
      if (loanCreated) {
        this.getMonographLoansDto();
      }
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  private getMonographLoansDto(): void {
    this.monographLoanService.getAll().subscribe({
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
        //
        this.dataSource.data = response.result as MonographLoanDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error?.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      }
    })
  }

  deleteMonographLoanClick(monographLoan: MonographLoanDto) {
    if (monographLoan.state?.trimEnd() !== "CREADA") {
      this.toastr.warning(`No es posible eliminar una solicitud préstada o devuelta`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }

    // Abrir dialogo para preguntar si desea eliminar el registro
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '300px',
      disableClose: true,
      data: monographLoan.monographLoanId
    });
    //
    dialogRef.afterClosed().subscribe((done) => {
      if (!done)
        return

      // Realizar solicitud para eliminar registro
      this.monographLoanService.delete(monographLoan.monographLoanId).subscribe({
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

          this.getMonographLoansDto();
        },
        error: (error: ApiResponse) => {
          this.toastr.error(`${error.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          });
        }
      })
    });
  }

  updateBorrowedMonographLoanClick(monographLoan: MonographLoanDto) {
    if (monographLoan.state?.trimEnd() !== "CREADA") {
      this.toastr.warning(`No es posible préstar la monografía sino se ha solicitado un préstamo`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }
    //
    const loanDto: UpdateBorrowedMonographLoanDto = {
      monographLoanId: monographLoan.monographLoanId,
      dueDate: new Date(),
      borrowedUserId: 1
    }
    // Realizar solicitud para prestar registro
    this.monographLoanService.updateBorrowedMonographLoan(loanDto).subscribe({
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

        this.getMonographLoansDto();
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      }
    })
  }
  
  updateReturnedMonographLoanClick(monographLoan: MonographLoanDto) {
    if (monographLoan.state?.trimEnd() !== "PRESTADA") {
      this.toastr.warning(`No es posible devolver la monografía si no esta PRESTADA`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }
    //
    const loanDto: UpdateReturnedMonographLoanDto = {
      monographLoanId: monographLoan.monographLoanId,
      returnedUserId: 1
    }
    // Realizar solicitud para devolver registro
    this.monographLoanService.updateReturnedMonographLoan(loanDto).subscribe({
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

        this.getMonographLoansDto();
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      }
    })
  }

}
