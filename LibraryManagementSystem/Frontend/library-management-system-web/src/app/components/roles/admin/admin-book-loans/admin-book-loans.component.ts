import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog'
import { BookLoanDto } from '../../../../entities/dtos/library/book-loan-dto';
import { BookLoanService } from '../../../../services/library/book-loan.service';
import { ToastrService } from 'ngx-toastr';
import { ApiResponse } from '../../../../entities/api/api-response';
import { DeleteDialogComponent } from '../../../delete-dialog/delete-dialog.component';
import { BookLoanSignalRService } from '../../../../services/signalr-hubs/book-loan-signal-r.service';
import { UpdateBorrowedBookLoanDto } from '../../../../entities/dtos/library/update-borrowed-book-loan-dto';
import { UpdateReturnedBookLoanDto } from '../../../../entities/dtos/library/update-returned-book-loan-dto';
import { BookCardComponent } from '../../../custom-cards/book-card/book-card.component';
import { StudentCardComponent } from '../../../custom-cards/student-card/student-card.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-book-loans',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule, BookCardComponent, StudentCardComponent,
    CommonModule
  ],
  templateUrl: './admin-book-loans.component.html',
  styleUrl: './admin-book-loans.component.css'
})
export class AdminBookLoansComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['bookLoanId', 'typeOfLoan', 'state', 'loanDate', 'dueDate', 'returnDate', 'student', 'book', 'borrowedUser', 'returnedUser', 'options'];

  /*  */
  dataSource: MatTableDataSource<BookLoanDto> = new MatTableDataSource<BookLoanDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;

  constructor(private bookLoanService: BookLoanService, private toastr: ToastrService, private dialog: MatDialog, private blSignalR: BookLoanSignalRService) { }

  ngOnInit(): void {
    this.getBookLoansDto();
    // Conectarse al Hub de libros
    this.blSignalR.bookLoanNotification.subscribe((loanCreated: boolean) => {
      if (loanCreated) {
        this.getBookLoansDto();
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

  private getBookLoansDto(): void {
    this.bookLoanService.getAll().subscribe({
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
        // Asignar datos
        this.dataSource.data = response.result as BookLoanDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error?.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      }
    })
  }

  deleteBookLoanClick(bookLoan: BookLoanDto) {
    if (bookLoan.state?.trimEnd() !== "CREADA") {
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
      data: bookLoan.bookLoanId
    });
    //
    dialogRef.afterClosed().subscribe((done) => {
      if (!done)
        return

      // Realizar solicitud para eliminar registro
      this.bookLoanService.delete(bookLoan.bookLoanId).subscribe({
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

          this.getBookLoansDto();
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

  updateBorrowedBookLoanClick(bookLoan: BookLoanDto) {
    if (bookLoan.state?.trimEnd() !== "CREADA") {
      this.toastr.warning(`No es posible préstar el libro sino se ha solicitado un préstamo`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }
    // 
    const loanDto: UpdateBorrowedBookLoanDto = {
      bookLoanId: bookLoan.bookLoanId,
      dueDate: new Date(),
      borrowedUserId: 1
    }

    // Realizar solicitud para prestar registro
    this.bookLoanService.updateBorrowedBookLoan(loanDto).subscribe({
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

        this.getBookLoansDto();
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      }
    })
  }

  updateReturnedBookLoanClick(bookLoan: BookLoanDto) {
    if (bookLoan.state?.trimEnd() !== "PRESTADO") {
      this.toastr.warning(`No es posible devolver el libro si no esta PRESTADO`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }
    //
    const loanDto: UpdateReturnedBookLoanDto = {
      bookLoanId: bookLoan.bookLoanId,
      returnedUserId: 1
    }
    // Realizar solicitud para devolver registro
    this.bookLoanService.updateReturnedBookLoan(loanDto).subscribe({
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

        this.getBookLoansDto();
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

