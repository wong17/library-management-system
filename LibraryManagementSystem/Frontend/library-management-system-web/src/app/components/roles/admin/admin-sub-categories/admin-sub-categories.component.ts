import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog'
import { SubCategoryDto } from '../../../../entities/dtos/library/sub-category-dto';
import { SubCategoryService } from '../../../../services/library/sub-category.service';
import { ToastrService } from 'ngx-toastr';
import { DialogData, DialogOperation } from '../../../../util/dialog-data';
import { AdminSubCategoriesDialogComponent } from '../admin-sub-categories-dialog/admin-sub-categories-dialog.component';
import { DeleteDialogComponent } from '../../../delete-dialog/delete-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiResponse } from '../../../../entities/api/api-response';

@Component({
  selector: 'app-admin-sub-categories',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule],
  templateUrl: './admin-sub-categories.component.html',
  styleUrl: './admin-sub-categories.component.css'
})
export class AdminSubCategoriesComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['id', 'name', 'categoryName', 'options'];

  /*  */
  dataSource: MatTableDataSource<SubCategoryDto> = new MatTableDataSource<SubCategoryDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;
  /* Editoriales */
  publishers: SubCategoryDto[] | undefined;

  constructor(private subCategoryService: SubCategoryService, private dialog: MatDialog, private toastr: ToastrService) { }

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

  ngOnInit(): void {
    this.getSubCategoriesDto();
  }

  insertSubCategoryClick(): void {
    // data
    const dialogData: DialogData = {
      title: 'Agregar nueva sub cátegoria',
      operation: DialogOperation.Add
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminSubCategoriesDialogComponent, {
      width: '500px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getSubCategoriesDto();
      }
    });
  }

  deleteSubCategoryClick(element: SubCategoryDto) {
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
      this.subCategoryService.delete(element.subCategoryId).subscribe({
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

          this.getSubCategoriesDto();
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

  editSubCategoryClick(element: SubCategoryDto) {
    // data
    const dialogData: DialogData = {
      title: `Editar sub cátegoria ${element.name}`,
      operation: DialogOperation.Update,
      data: element
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminSubCategoriesDialogComponent, {
      width: '500px',
      disableClose: true,
      data: dialogData
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done) {
        this.getSubCategoriesDto();
      }
    });
  }

  private getSubCategoriesDto(): void {
    this.subCategoryService.getAll().subscribe({
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
        const list: SubCategoryDto[] = response.result as SubCategoryDto[];
        this.dataSource.data = list;
      },
      error: error => {
        console.error(error);
      }
    })
  }

}