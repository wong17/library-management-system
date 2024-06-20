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
import { MonographDto } from '../../../../entities/dtos/library/monograph-dto';

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

  updateReturnedMonographLoanClick(monograph: MonographDto) {

  }
  updateBorrowedMonographLoanClick(monograph: MonographDto) {

  }
  deleteMonographLoanClick(monograph: MonographDto) {

  }

}
