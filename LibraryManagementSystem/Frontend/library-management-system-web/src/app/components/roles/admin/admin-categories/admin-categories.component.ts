import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog'
import { CategoryService } from '../../../../services/library/category.service';
import { CategoryDto } from '../../../../entities/dtos/library/category-dto';
import { ToastrService } from 'ngx-toastr';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { AdminCategoriesDialogComponent } from '../admin-categories-dialog/admin-categories-dialog.component';
import { DeleteDialogComponent } from '../../../delete-dialog/delete-dialog.component';
import { ApiResponse } from '../../../../entities/api/api-response';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CategorySignalRService } from '../../../../services/signalr-hubs/category-signal-r.service';

@Component({
  selector: 'app-admin-categories',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule, MatTooltipModule],
  templateUrl: './admin-categories.component.html',
  styleUrl: './admin-categories.component.css'
})
export class AdminCategoriesComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['id', 'name', 'options'];

  /*  */
  dataSource: MatTableDataSource<CategoryDto> = new MatTableDataSource<CategoryDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;

  constructor(
    private categoryService: CategoryService, 
    private dialog: MatDialog, 
    private toastr: ToastrService,
    private cSignalR: CategorySignalRService
  ) {
   }

  ngOnInit(): void {
    this.getCategoriesDto();
    // Conectarse al Hub de monografias
    this.cSignalR.categoryNotification.subscribe((value: boolean) => {
      this.getCategoriesDto();
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

  insertCategoryClick(): void {
    // data
    const dialogData: DialogData = {
      title: 'Agregar nueva categoría',
      operation: DialogOperation.Add
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminCategoriesDialogComponent, {
      width: '500px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getCategoriesDto();
      }
    });
  }

  deleteCategoryClick(element: CategoryDto) {
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
      this.categoryService.delete(element.categoryId).subscribe({
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

          this.getCategoriesDto();
        },
        error: (error: ApiResponse) => {
          this.toastr.error(`${error.message}`, 'Error', {
            timeOut: 5000
          });
        }
      })
    });
  }

  editCategoryClick(element: CategoryDto) {
    // data
    const dialogData: DialogData = {
      title: `Editar categoría ${element.name}`,
      operation: DialogOperation.Update,
      data: element
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminCategoriesDialogComponent, {
      width: '500px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getCategoriesDto();
      }
    });
  }

  private getCategoriesDto(): void {
    this.categoryService.getAll().subscribe({
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
        this.dataSource.data = list as CategoryDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

}
