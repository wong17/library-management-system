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
import { MatCheckbox } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthorDto } from '../../../../entities/dtos/library/author-dto';
import { AuthorService } from '../../../../services/library/author.service';
import { StringUtil } from '../../../../util/string-util';
import { CareerDto } from '../../../../entities/dtos/university/career-dto';
import { CareerService } from '../../../../services/university/career.service';
import { MatSelectModule } from '@angular/material/select';
import { FilterMonographDto } from '../../../../entities/dtos/library/filter-monograph-dto';

@Component({
  selector: 'app-admin-monographs',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule,
    MatCheckbox, MatChipsModule, MatTooltipModule, CommonModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './admin-monographs.component.html',
  styleUrl: './admin-monographs.component.css'
})
export class AdminMonographsComponent implements AfterViewInit, OnInit {
  /* */
  filterForm: FormGroup;

  displayedColumns: string[] = ['monographId', 'image', 'classification', 'title', 'description', 'tutor', 'presentationDate',
    'careerName', 'authors', 'isAvailable', 'isActive', 'options'];

  /*  */
  dataSource: MatTableDataSource<MonographDto> = new MatTableDataSource<MonographDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;
  /* Autores */
  authors: AuthorDto[] | undefined;
  /* Carreras */
  careers: CareerDto[] | undefined;
  /* */
  filterMonographDto: FilterMonographDto = { authors: null, careers: null, beginPresentationDate: null, endPresentationDate: null }

  constructor(
    private monographService: MonographService,
    private authorService: AuthorService,
    private careerService: CareerService,
    private dialog: MatDialog,
    private toastr: ToastrService,
    private fb: FormBuilder
  ) {
    this.filterForm = this.fb.group({
      authorIds: [''],
      careerIds: [''],
      endPresentationDate: [''],
      beginPresentationDate: ['']
    });
  }

  ngOnInit(): void {
    this.loadData();
    // Configure the filterPredicate to use removeAccents
    this.dataSource.filterPredicate = (data: MonographDto, filter: string) => {
      const dataStr = StringUtil.removeAccents(JSON.stringify(data).toLowerCase());
      return dataStr.includes(filter);
    };
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = StringUtil.removeAccents(filterValue.trim().toLowerCase());

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  private async loadData(): Promise<void> {
    await Promise.all([
      this.getMonographsDto(),
      this.getAuthorsDto(),
      this.getCareersDto()
    ]);
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
              timeOut: 5000
            })
            return;
          }
          // Solicitud exitosa
          this.toastr.success(`${response.message}`, 'Exito', {
            timeOut: 5000
          })

          this.getMonographsDto();
        },
        error: (error: ApiResponse) => {
          this.toastr.error(`${error.message}`, 'Error', {
            timeOut: 5000
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

  onSubmit() {
    // Obtener autores, editoriales, categorias, subcategorias y año seleccionado
    const selectedAuthors = Array.isArray(this.filterForm.get('authorIds')?.value) ? this.filterForm.get('authorIds')?.value : [];
    const selectedCareers = Array.isArray(this.filterForm.get('careerIds')?.value) ? this.filterForm.get('careerIds')?.value : [];

    debugger;

    this.filterMonographDto = {
      authors: selectedAuthors.map((id: number) => ({ authorId: id, name: "", isFormerGraduated: false })),
      careers: selectedCareers.map((id: number) => ({ careerId: id, name: "" })),
      beginPresentationDate: this.beginPresentationDate?.value === '' ? null : this.beginPresentationDate?.value as Date,
      endPresentationDate: this.endPresentationDate?.value === '' ? null : this.endPresentationDate?.value as Date
    };

    // realizar solicitud a la api
    this.monographService.getFilteredMonograph(this.filterMonographDto).subscribe({
      next: response => {
        // Verificar si ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
          return;
        }
        // Verificar si el resultado es un array válido
        const list = response.result;
        if (!Array.isArray(list)) {
          this.toastr.error(`El resultado no es un array válido: ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
          return;
        }
        // Asignar datos
        this.dataSource.data = response.result as MonographDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  reloadMonographsClick() {
    this.loadData();
  }

  private getMonographsDto(): void {
    this.monographService.getAll().subscribe({
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
        this.dataSource.data = response.result as MonographDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  // Obtener autores de la api
  private getAuthorsDto(): void {
    this.authorService.getAll().subscribe({
      next: response => {
        // Verificar si ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
          return;
        }
        // Verificar si el resultado es un array válido
        const list = response.result;
        if (!Array.isArray(list)) {
          this.toastr.error(`El resultado no es un array válido: ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
          return;
        }
        // Asignar datos
        this.authors = list as AuthorDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      }
    })
  }

  // Obtener carreras de la api
  private getCareersDto(): void {
    this.careerService.getAll().subscribe({
      next: response => {
        // Verificar si ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.error(`Ocurrio un error ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
          return;
        }
        // Verificar si el resultado es un array válido
        const list = response.result;
        if (!Array.isArray(list)) {
          this.toastr.error(`El resultado no es un array válido: ${response.message}`, 'Error', {
            timeOut: 5000,
            easeTime: 1000
          })
          return;
        }
        // Asignar datos
        this.careers = list as CareerDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
        });
      }
    })
  }

  /* Getters */
  get authorIds() {
    return this.filterForm.get('authorIds');
  }

  get careerIds() {
    return this.filterForm.get('careerIds');
  }

  get beginPresentationDate() {
    return this.filterForm.get('beginPresentationDate');
  }

  get endPresentationDate() {
    return this.filterForm.get('endPresentationDate');
  }

}
