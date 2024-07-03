import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog'
import { PublisherService } from '../../../../services/library/publisher.service';
import { PublisherDto } from '../../../../entities/dtos/library/publisher-dto';
import { AdminPublishersDialogComponent } from '../admin-publishers-dialog/admin-publishers-dialog.component';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { ToastrService } from 'ngx-toastr';
import { DeleteDialogComponent } from '../../../delete-dialog/delete-dialog.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ApiResponse } from '../../../../entities/api/api-response';

@Component({
  selector: 'app-admin-publishers',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule, MatTooltipModule],
  templateUrl: './admin-publishers.component.html',
  styleUrl: './admin-publishers.component.css'
})
export class AdminPublishersComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['id', 'name', 'options'];

  /*  */
  dataSource: MatTableDataSource<PublisherDto> = new MatTableDataSource<PublisherDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;

  constructor(private publisherService: PublisherService, private dialog: MatDialog, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getPublishersDto();
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

  insertPublisherClick() {
    // data
    const dialogData: DialogData = {
      title: 'Agregar nueva editorial',
      operation: DialogOperation.Add
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminPublishersDialogComponent, {
      width: '500px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getPublishersDto();
      }
    });
  }

  deletePublisherClick(element: PublisherDto) {
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
      this.publisherService.delete(element.publisherId).subscribe({
        next: response => {
          // Ocurrio un error
          if (response.isSuccess !== 0 || response.statusCode !== 200) {
            this.toastr.error(`${response.message}`, 'Error', {
              timeOut: 5000
            })
            return;
          }
          // Solicitud exitosa
          this.toastr.success(`${response.message}`, 'Exito', {
            timeOut: 5000
          })

          this.getPublishersDto();
        },
        error: error => {
          this.toastr.error(error.message, 'Error', {
            timeOut: 5000
          });
        }
      })
    });
  }

  editPublisherClick(element: PublisherDto) {
    // data
    const dialogData: DialogData = {
      title: `Editar editorial ${element.name}`,
      operation: DialogOperation.Update,
      data: element
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminPublishersDialogComponent, {
      width: '500px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getPublishersDto();
      }
    });
  }

  private getPublishersDto(): void {
    this.publisherService.getAll().subscribe({
      next: response => {
        // Verificar si ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`${response.message}`, 'Error', {
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
        this.dataSource.data = list as PublisherDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    });
  }

}
