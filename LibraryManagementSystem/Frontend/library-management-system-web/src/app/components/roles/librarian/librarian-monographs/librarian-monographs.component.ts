import { Component, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MonographDto } from '../../../../entities/dtos/library/monograph-dto';
import { MatSort } from '@angular/material/sort';
import { MonographService } from '../../../../services/library/monograph.service';
import { ToastrService } from 'ngx-toastr';
import { ApiResponse } from '../../../../entities/api/api-response';
import { MatChipsModule } from '@angular/material/chips';
import { MatCheckbox } from '@angular/material/checkbox';
import { AuthorService } from '../../../../services/library/author.service';
import { CareerService } from '../../../../services/university/career.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthorDto } from '../../../../entities/dtos/library/author-dto';
import { CareerDto } from '../../../../entities/dtos/university/career-dto';
import { FilterMonographDto } from '../../../../entities/dtos/library/filter-monograph-dto';
import { StringUtil } from '../../../../util/string-util';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-librarian-monographs',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule, MatCheckbox, 
    MatChipsModule, MatTooltipModule, CommonModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './librarian-monographs.component.html',
  styleUrl: './librarian-monographs.component.css'
})
export class LibrarianMonographsComponent {

  /* */
  filterForm: FormGroup;

  displayedColumns: string[] = ['image', 'classification', 'title', 'description', 'tutor', 'presentationDate', 'careerName', 'isAvailable', 'authors'];

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
        this.authors = list.filter((author: AuthorDto) => author.isFormerGraduated);
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
