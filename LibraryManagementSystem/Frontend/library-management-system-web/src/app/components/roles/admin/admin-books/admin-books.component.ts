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
import { ToastrService } from 'ngx-toastr';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { DeleteDialogComponent } from '../../../delete-dialog/delete-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../../../../entities/api/api-response';
import { AdminBooksDialogComponent } from '../admin-books-dialog/admin-books-dialog.component';

@Component({
  selector: 'app-admin-books',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule],
  templateUrl: './admin-books.component.html',
  styleUrl: './admin-books.component.css'
})
export class AdminBooksComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['bookId', 'isbN10', 'isbN13', 'classification', 'title', 'description', 'publicationYear', 'isActive', 'numberOfCopies', 'borrowedCopies', 'isAvailable',
    'publisherName', 'categoryName', 'authors', 'subCategories', 'options'];

  /*  */
  dataSource: MatTableDataSource<BookDto> = new MatTableDataSource<BookDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;
  /* Libros */
  books: BookDto[] | undefined;

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

  insertBookClick(): void {
    // data
    const dialogData: DialogData = {
      title: 'Agregar nuevo libro',
      operation: DialogOperation.Add
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminBooksDialogComponent, {
      width: '800px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getBooksDto();
      }
    });
  }

  deleteBookClick(element: BookDto) {
    // Abrir dialogo para preguntar si desea eliminar el registro
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '300px',
      disableClose: true,
      data: element.title
    });
    //
    dialogRef.afterClosed().subscribe((done) => {
      if (!done)
        return

      // Realizar solicitud para eliminar registro
      this.bookService.delete(element.bookId).subscribe({
        next: response => {
          // Ocurrio un error
          if (response.isSuccess !== 0 || response.statusCode !== 200) {
            this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
              timeOut: 3000,
              easeTime: 1000
            })
            return;
          }
          // Solicitud exitosa
          this.toastr.success(`${response.message}`, 'Exito', {
            timeOut: 3000,
            easeTime: 1000
          })

          this.getBooksDto();
        },
        error: error => {
          if (error instanceof HttpErrorResponse) {
            //
            const response = error.error as ApiResponse;
            // BadRequest
            if (response.isSuccess === 1 && response.statusCode === 400) {
              this.toastr.warning(`${response.message}`, 'Atención', {
                timeOut: 3000,
                easeTime: 1000
              });
              return;
            }
            // InternalServerError
            if (response.isSuccess === 3 && response.statusCode === 500) {
              this.toastr.error(`${response.message}`, 'Error', {
                timeOut: 3000,
                easeTime: 1000
              });
            }
          }
        }
      })
    });
  }

  editBookClick(element: BookDto) {
    // data
    const dialogData: DialogData = {
      title: `Editar libro ${element.title}`,
      operation: DialogOperation.Update,
      data: element
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminBooksDialogComponent, {
      width: '800px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getBooksDto();
      }
    });
  }

  private getBooksDto(): void {
    this.bookService.getAll().subscribe({
      next: response => {
        // Verificar si la respuesta es nula
        if (!response) {
          this.toastr.error('No se recibió respuesta del servidor', 'Error', {
            timeOut: 3000,
            easeTime: 1000
          })
          return;
        }
        // Verificar si ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 3000,
            easeTime: 1000
          })
          return;
        }
        // Verificar si el resultado es un array válido
        const list = response.result;
        if (!Array.isArray(list)) {
          this.toastr.error(`El resultado no es un array válido: ${response.message}`, 'Error', {
            timeOut: 3000,
            easeTime: 1000
          })
          return;
        }
        // Asignar datos
        this.dataSource.data = response.result as BookDto[];
      },
      error: error => {
        if (error instanceof HttpErrorResponse) {
          //
          const response = error.error as ApiResponse;
          // BadRequest
          if (response.isSuccess === 1 && response.statusCode === 400) {
            this.toastr.warning(`${response.message}`, 'Atención', {
              timeOut: 3000,
              easeTime: 1000
            });
            return;
          }
          // InternalServerError
          if (response.isSuccess === 3 && response.statusCode === 500) {
            this.toastr.error(`${response.message}`, 'Error', {
              timeOut: 3000,
              easeTime: 1000
            });
          }
        }
      }
    })
  }

  
}
