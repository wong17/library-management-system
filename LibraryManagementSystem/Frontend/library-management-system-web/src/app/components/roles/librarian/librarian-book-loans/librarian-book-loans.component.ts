import { Component, ViewChild } from '@angular/core';
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

@Component({
  selector: 'app-librarian-book-loans',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule],
  templateUrl: './librarian-book-loans.component.html',
  styleUrl: './librarian-book-loans.component.css'
})
export class LibrarianBookLoansComponent {

  displayedColumns: string[] = ['bookLoanId', 'typeOfLoan', 'state', 'loanDate', 'dueDate', 'returnDate', 'student', 'book'];

  /*  */
  dataSource: MatTableDataSource<BookLoanDto> = new MatTableDataSource<BookLoanDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;
  /* Editoriales */
  publishers: BookLoanDto[] | undefined;

  constructor(private bookLoanService: BookLoanService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getBookLoansDto();
  }
  
  private getBookLoansDto(): void {
    this.bookLoanService.getAll().subscribe({
      next: response => {
        //
        if (!response) return;
        //
        if (response.isSuccess !== 1 && response.statusCode !== 200) {
          console.error(`Message: ${response.message}, StatusCode: ${response.statusCode}`);
          return;
        }
        //
        if (response.result === null || !Array.isArray(response.result)) {
          console.error(`Message: ${response.message}, StatusCode: ${response.statusCode}`);
          return;
        }
        //
        const list: BookLoanDto[] = response.result as BookLoanDto[];
        this.dataSource.data = list;
      },
      error: error => {
        console.error(error);
      }
    })
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

}
