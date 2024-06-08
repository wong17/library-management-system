import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog'
import { BookDto } from '../../../../entities/dtos/library/book-dto';
import { BookService } from '../../../../services/library/book.service';

@Component({
  selector: 'app-admin-books',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule],
  templateUrl: './admin-books.component.html',
  styleUrl: './admin-books.component.css'
})
export class AdminBooksComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['bookId', 'isbN10', 'isbN13', 'classification', 'title', 'description', 'publicationYear', 'isActive', 'numberOfCopies', 'borrowedCopies', 'isAvailable',
    'publisherName', 'categoryName', 'authors', 'subCategories'
  ];

  /*  */
  dataSource: MatTableDataSource<BookDto> = new MatTableDataSource<BookDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;
  /* Editoriales */
  publishers: BookDto[] | undefined;

  constructor(private bookService: BookService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getBooksDto();
  }
  
  private getBooksDto(): void {
    this.bookService.getAll().subscribe({
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
        const list: BookDto[] = response.result as BookDto[];
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

  insertNewBookBtn() {

  }

}