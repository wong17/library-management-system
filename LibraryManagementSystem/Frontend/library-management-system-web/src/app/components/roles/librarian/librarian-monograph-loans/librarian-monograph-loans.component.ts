import { Component, ViewChild } from '@angular/core';
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

@Component({
  selector: 'app-librarian-monograph-loans',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule],
  templateUrl: './librarian-monograph-loans.component.html',
  styleUrl: './librarian-monograph-loans.component.css'
})
export class LibrarianMonographLoansComponent {

  displayedColumns: string[] = ['monographLoanId', 'state', 'loanDate', 'dueDate', 'returnDate', 'student', 'monograph'];

  /*  */
  dataSource: MatTableDataSource<MonographLoanDto> = new MatTableDataSource<MonographLoanDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;
  /* Editoriales */
  publishers: MonographLoanDto[] | undefined;

  constructor(private monographLoanService: MonographLoanService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getMonographLoansDto();
  }

  private getMonographLoansDto(): void {
    this.monographLoanService.getAll().subscribe({
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
        const list: MonographLoanDto[] = response.result as MonographLoanDto[];
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
