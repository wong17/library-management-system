import { Component, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { BookDto } from '../../../../entities/dtos/library/book-dto';
import { ApiResponse } from '../../../../entities/api/api-response';
import { BookService } from '../../../../services/library/book.service';
import { ToastrService } from 'ngx-toastr';
import { MatSort } from '@angular/material/sort';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { StudentBooksLoansDialogComponent } from '../student-books-loans-dialog/student-books-loans-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-student-books',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule],
  templateUrl: './student-books.component.html',
  styleUrl: './student-books.component.css'
})
export class StudentBooksComponent {

  displayedColumns: string[] = ['isbN10', 'isbN13', 'classification', 'title', 'description', 'publicationYear', 'publisherName', 
    'categoryName', 'authors', 'subCategories', 'options'];

  /*  */
  dataSource: MatTableDataSource<BookDto> = new MatTableDataSource<BookDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;
  /* Libros */
  books: BookDto[] = [];

  constructor(private bookService: BookService, private dialog: MatDialog, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getBooksDto();
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

  private getBooksDto(): void {
    this.bookService.getAll().subscribe({
      next: response => {
        // Verificar si ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 5000
          })
          return;
        }
        // Verificar si el resultado es un array válido
        const list = response.result;
        if (!Array.isArray(list)) {
          this.toastr.error(`El resultado no es un array válido: ${response.message}`, 'Error', {
            timeOut: 5000
          })
          return;
        }
        // Asignar datos
        this.dataSource.data = response.result as BookDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message} ${error.isSuccess}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  requestBookClick(book: BookDto) {
    // data
    const dialogData: DialogData = {
      title: 'Préstamo de libro',
      operation: DialogOperation.Add,
      data: book
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(StudentBooksLoansDialogComponent, {
      width: '800px',
      disableClose: true,
      data: dialogData
    });
  }

}
