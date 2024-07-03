import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MonographLoanDto } from '../../../../entities/dtos/library/monograph-loan-dto';
import { MatSort } from '@angular/material/sort';
import { MonographLoanService } from '../../../../services/library/monograph-loan.service';
import { MatDialog } from '@angular/material/dialog';
import { ApiResponse } from '../../../../entities/api/api-response';
import { DeleteDialogComponent } from '../../../delete-dialog/delete-dialog.component';
import { ToastrService } from 'ngx-toastr';
import { MonographLoanSignalRService } from '../../../../services/signalr-hubs/monograph-loan-signal-r.service';
import { CommonModule } from '@angular/common';
import { MonographCardComponent } from '../../../custom-cards/monograph-card/monograph-card.component';
import { StudentCardComponent } from '../../../custom-cards/student-card/student-card.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MonographReturnDialogComponent } from '../../../monograph-return-dialog/monograph-return-dialog.component';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { MonographBorrowDateLoanComponent } from '../../../monograph-borrow-dialog/monograph-borrow-date-loan.component';

@Component({
  selector: 'app-librarian-monograph-loans',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule,
    StudentCardComponent, MonographCardComponent, CommonModule, MatTooltipModule
  ],
  templateUrl: './librarian-monograph-loans.component.html',
  styleUrl: './librarian-monograph-loans.component.css'
})
export class LibrarianMonographLoansComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['monographLoanId', 'state', 'loanDate', 'dueDate', 'returnDate', 'student', 'monograph', 'borrowedUser', 'returnedUser', 'options'];

  /*  */
  dataSource: MatTableDataSource<MonographLoanDto> = new MatTableDataSource<MonographLoanDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;

  /**
   * Inicializa el componente
   * @param monographLoanService 
   * @param toastr 
   * @param dialog 
   * @param mlSignalR 
   */
  constructor(
    private monographLoanService: MonographLoanService, 
    private toastr: ToastrService, 
    private dialog: MatDialog, 
    private mlSignalR: MonographLoanSignalRService
  ) { }

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

  /**
   * Obtine todas las solicitudes de prestamo de monografias de la bd
   */
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

  /**
   * Elimina la solicitud seleccionada en la vista
   * @param monographLoan 
   * @returns 
   */
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

  /**
   * Abre la ventana de dialogo para preguntar si se desea prestar la monografia
   * @param monographLoan 
   * @returns 
   */
  updateBorrowedMonographLoanClick(monographLoan: MonographLoanDto) {
    // Validar estado de la solicitud
    if (monographLoan.state?.trimEnd() !== "CREADA") {
      this.toastr.warning(`No es posible préstar la monografía sino se ha solicitado un préstamo`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }
    
    const dialogData: DialogData = {
      title: `Préstamo de monografía`,
      operation: DialogOperation.Add,
      data: monographLoan
    };

    // Abrir dialogo para preguntar si desea devolver el libro o no
    const dialogRef = this.dialog.open(MonographBorrowDateLoanComponent, {
      width: '400px',
      disableClose: true,
      data: dialogData
    });

    //
    dialogRef.afterClosed().subscribe((done) => {
      if (!done)
        return

      this.getMonographLoansDto();
    })
  }
  
  /**
   * Abre la ventana de dialogo para preguntar si se desea devolver la monografia
   * @param monographLoan 
   * @returns 
   */
  updateReturnedMonographLoanClick(monographLoan: MonographLoanDto) {
    // Validar estado de la solicitud
    if (monographLoan.state?.trimEnd() !== "PRESTADA") {
      this.toastr.warning(`No es posible devolver la monografía si no esta PRESTADA`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }

    const dialogData: DialogData = {
      title: `Devolución de monografía`,
      operation: DialogOperation.Add,
      data: monographLoan
    };

    // Abrir dialogo para preguntar si desea devolver el libro o no
    const dialogRef = this.dialog.open(MonographReturnDialogComponent, {
      width: '400px',
      disableClose: true,
      data: dialogData
    });

    //
    dialogRef.afterClosed().subscribe((done) => {
      if (!done)
        return

      this.getMonographLoansDto();
    })
  }

}
