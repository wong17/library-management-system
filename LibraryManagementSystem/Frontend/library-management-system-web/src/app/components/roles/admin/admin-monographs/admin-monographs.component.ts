import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog'
import { MonographDto } from '../../../../entities/dtos/library/monograph-dto';
import { MonographService } from '../../../../services/library/monograph.service';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { AdminMonographsDialogComponent } from '../admin-monographs-dialog/admin-monographs-dialog.component';
import { DeleteDialogComponent } from '../../../delete-dialog/delete-dialog.component';
import { ToastrService } from 'ngx-toastr';
import { ApiResponse } from '../../../../entities/api/api-response';
import { AdminMonographsAuthorsDialogComponent } from '../admin-monographs-authors-dialog/admin-monographs-authors-dialog.component';

@Component({
  selector: 'app-admin-monographs',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule],
  templateUrl: './admin-monographs.component.html',
  styleUrl: './admin-monographs.component.css'
})
export class AdminMonographsComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['monographId', 'classification', 'title', 'description', 'tutor', 'presentationDate', 'isActive', 'isAvailable', 'careerName', 'authors', 
    'options'];

  /*  */
  dataSource: MatTableDataSource<MonographDto> = new MatTableDataSource<MonographDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;

  constructor(private monographService: MonographService, private dialog: MatDialog, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getMonographsDto();
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

  insertMonographClick(): void {
    // data
    const dialogData: DialogData = {
      title: 'Agregar nueva monografía',
      operation: DialogOperation.Add
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminMonographsDialogComponent, {
      width: '800px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getMonographsDto();
      }
    });
  }

  deleteMonographClick(element: MonographDto) {
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
      this.monographService.delete(element.monographId).subscribe({
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

          this.getMonographsDto();
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

  editMonographClick(element: MonographDto) {
    // data
    const dialogData: DialogData = {
      title: `Editar monografía ${element.title}`,
      operation: DialogOperation.Update,
      data: element
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminMonographsDialogComponent, {
      width: '800px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getMonographsDto();
      }
    });
  }

  editMonographAuthorsClick(element: MonographDto) {
    // data
    const dialogData: DialogData = {
      title: element.authors?.length === 0 ? `Agregar autores de ${element.title}` : `Editar autores de ${element.title}`,
      operation: element.authors?.length === 0 ? DialogOperation.Add : DialogOperation.Update,
      data: element
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminMonographsAuthorsDialogComponent, {
      width: '500px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getMonographsDto();
      }
    });
  }

  private getMonographsDto(): void {
    this.monographService.getAll().subscribe({
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
        const list: MonographDto[] = response.result as MonographDto[];
        this.dataSource.data = list;
      },
      error: error => {
        console.error(error);
      }
    })
  }

}
