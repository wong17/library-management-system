import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog'
import { AuthorDto } from '../../../../entities/dtos/library/author-dto';
import { AuthorService } from '../../../../services/library/author.service';
import { ToastrService } from 'ngx-toastr';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { AdminAuthorsDialogComponent } from '../admin-authors-dialog/admin-authors-dialog.component';
import { DeleteDialogComponent } from '../../../delete-dialog/delete-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../../../../entities/api/api-response';

@Component({
  selector: 'app-admin-authors',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule],
  templateUrl: './admin-authors.component.html',
  styleUrl: './admin-authors.component.css'
})
export class AdminAuthorsComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['id', 'name', 'isFormerGraduated', 'options'];

  /*  */
  dataSource: MatTableDataSource<AuthorDto> = new MatTableDataSource<AuthorDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;

  constructor(private authorService: AuthorService, private dialog: MatDialog, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getAuthorsDto();
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

  insertAuthorClick(): void {
    // data
    const dialogData: DialogData = {
      title: 'Agregar nuevo autor',
      operation: DialogOperation.Add
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminAuthorsDialogComponent, {
      width: '500px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getAuthorsDto();
      }
    });
  }

  deleteAuthorClick(element: AuthorDto) {
    // Abrir dialogo para preguntar si desea eliminar el registro
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      width: '300px',
      disableClose: true,
      data: element.name
    });
    //
    dialogRef.afterClosed().subscribe((done) => {
      if (!done)
        return

      // Realizar solicitud para eliminar registro
      this.authorService.delete(element.authorId).subscribe({
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

          this.getAuthorsDto();
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

  editAuthorClick(element: AuthorDto) {
    // data
    const dialogData: DialogData = {
      title: `Editar autor ${element.name}`,
      operation: DialogOperation.Update,
      data: element
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminAuthorsDialogComponent, {
      width: '500px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getAuthorsDto();
      }
    });
  }

  private getAuthorsDto(): void {
    this.authorService.getAll().subscribe({
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
        this.dataSource.data = list as AuthorDto[];
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