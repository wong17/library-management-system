import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { BookLoanDto } from '../../../../entities/dtos/library/book-loan-dto';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { BookLoanService } from '../../../../services/library/book-loan.service';
import { MatDialog } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ApiResponse } from '../../../../entities/api/api-response';
import { DeleteDialogComponent } from '../../../delete-dialog/delete-dialog.component';
import { ToastrService } from 'ngx-toastr';
import { BookLoanSignalRService } from '../../../../services/signalr-hubs/book-loan-signal-r.service';
import { BookCardComponent } from '../../../custom-cards/book-card/book-card.component';
import { StudentCardComponent } from '../../../custom-cards/student-card/student-card.component';
import { CommonModule } from '@angular/common';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BookBorrowDateLoanComponent } from '../../../book-borrow-date-loan/book-borrow-date-loan.component';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { BookReturnDialogComponent } from '../../../book-return-dialog/book-return-dialog.component';

@Component({
  selector: 'app-librarian-book-loans',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule, 
    BookCardComponent, StudentCardComponent, CommonModule, MatTooltipModule],
  templateUrl: './librarian-book-loans.component.html',
  styleUrl: './librarian-book-loans.component.css'
})
export class LibrarianBookLoansComponent implements OnInit, AfterViewInit {

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
    // Verificar estado de la solicitud
    if (bookLoan.state?.trimEnd() !== "CREADA") {
      this.toastr.warning(`No es posible prestar el libro sino se ha solicitado un préstamo`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }

    const dialogData: DialogData = {
      title: `Préstamo de libro`,
      operation: DialogOperation.Add,
      data: bookLoan
    };

    // Abrir dialogo para preguntar por fecha de devolución
    const dialogRef = this.dialog.open(BookBorrowDateLoanComponent, {
      width: '400px',
      disableClose: true,
      data: dialogData
    });
    //
    dialogRef.afterClosed().subscribe((done) => {
      if (!done)
        return

      this.getBookLoansDto();
    })
  }

  updateReturnedBookLoanClick(bookLoan: BookLoanDto) {
    // Verificar estado de la solicitud
    if (bookLoan.state?.trimEnd() !== "PRESTADO") {
      this.toastr.warning(`No es posible devolver el libro si no esta PRESTADO`, 'Atención', {
        timeOut: 5000,
        easeTime: 1000
      })
      return
    }
    
    const dialogData: DialogData = {
      title: `Devolución de libro`,
      operation: DialogOperation.Add,
      data: bookLoan
    };

    // Abrir dialogo para preguntar si desea devolver el libro o no
    const dialogRef = this.dialog.open(BookReturnDialogComponent, {
      width: '400px',
      disableClose: true,
      data: dialogData
    });

    //
    dialogRef.afterClosed().subscribe((done) => {
      if (!done)
        return

      this.getBookLoansDto();
    })
  }

}
