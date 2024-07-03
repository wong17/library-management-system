import { Component, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { BookDto } from '../../../../entities/dtos/library/book-dto';
import { MatSort } from '@angular/material/sort';
import { BookService } from '../../../../services/library/book.service';
import { ToastrService } from 'ngx-toastr';
import { ApiResponse } from '../../../../entities/api/api-response';
import { MatChipsModule } from '@angular/material/chips';
import { MatCheckbox } from '@angular/material/checkbox';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { SubCategoryDto } from '../../../../entities/dtos/library/sub-category-dto';
import { AuthorDto } from '../../../../entities/dtos/library/author-dto';
import { FilterBookDto } from '../../../../entities/dtos/library/filter-book-dto';
import { AuthorService } from '../../../../services/library/author.service';
import { PublisherService } from '../../../../services/library/publisher.service';
import { CategoryService } from '../../../../services/library/category.service';
import { SubCategoryService } from '../../../../services/library/sub-category.service';
import { StringUtil } from '../../../../util/string-util';
import { PublisherDto } from '../../../../entities/dtos/library/publisher-dto';
import { CategoryDto } from '../../../../entities/dtos/library/category-dto';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';
import { BookSignalRService } from '../../../../services/signalr-hubs/book-signal-r.service';

@Component({
  selector: 'app-librarian-books',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule, MatCheckbox, 
    MatChipsModule, ReactiveFormsModule, MatTooltipModule, MatSelectModule],
  templateUrl: './librarian-books.component.html',
  styleUrl: './librarian-books.component.css'
})
export class LibrarianBooksComponent {

  displayedColumns: string[] = ['image', 'isbN10', 'isbN13', 'classification', 'title', 'description', 'publicationYear', 'numberOfCopies', 'borrowedCopies', 'isAvailable',
    'publisherName', 'categoryName', 'authors', 'subCategories'];

  /* */
  filterForm: FormGroup;
  /*  */
  dataSource: MatTableDataSource<BookDto> = new MatTableDataSource<BookDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;
  /* Categorias */
  categories: CategoryDto[] | undefined;
  /* Editoriales */
  publishers: PublisherDto[] | undefined;
  /* Autores */
  authors: AuthorDto[] | undefined;
  /* Sub categorias */
  subCategories: SubCategoryDto[] | undefined;
  /* Años que se publicaron los libros */
  availableYears: number[] = [];

  filterBookDto: FilterBookDto = { authors: null, categories: null, publishers: null, subCategories: null, publicationYear: null };

  constructor(
    private bookService: BookService,
    private authorService: AuthorService,
    private publisherService: PublisherService,
    private categoryService: CategoryService,
    private subCategoryService: SubCategoryService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    private bSignalR: BookSignalRService
  ) {
    this.filterForm = this.fb.group({
      authorIds: [''],
      publisherIds: [''],
      publicationYear: [''],
      categoryIds: [''],
      subCategoryIds: ['']
    });
   }

  ngOnInit(): void {
    this.loadData();

    // Configure the filterPredicate to use removeAccents
    this.dataSource.filterPredicate = (data: BookDto, filter: string) => {
      const dataStr = StringUtil.removeAccents(JSON.stringify(data).toLowerCase());
      return dataStr.includes(filter);
    };

    // Conectarse al Hub de libros
    this.bSignalR.bookNotification.subscribe((value: boolean) => {
      this.getBooksDto();
    });
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
      this.getBooksDto(),
      this.getCategoriesDto(),
      this.getPublishersDto(),
      this.getAuthorsDto(),
      this.getSubCategoriesDto()
    ]);
    const currentYear = new Date().getFullYear();
    for (let year = currentYear; year >= 1900; year--) {
      this.availableYears.push(year);
    }
  }

  reloadBooksClick() {
    this.loadData();
  }

  onSubmit(): void {
    // Obtener autores, editoriales, categorias, subcategorias y año seleccionado
    const selectedPublishers = Array.isArray(this.filterForm.get('publisherIds')?.value) ? this.filterForm.get('publisherIds')?.value : [];
    const selectedAuthors = Array.isArray(this.filterForm.get('authorIds')?.value) ? this.filterForm.get('authorIds')?.value : [];
    const selectedCategories = Array.isArray(this.filterForm.get('categoryIds')?.value) ? this.filterForm.get('categoryIds')?.value : [];
    const selectedSubCategories = Array.isArray(this.filterForm.get('subCategoryIds')?.value) ? this.filterForm.get('subCategoryIds')?.value : [];

    this.filterBookDto = {
      authors: selectedAuthors.map((id: number) => ({ authorId: id, name: "", isFormerGraduated: false })),
      publishers: selectedPublishers.map((id: number) => ({ publisherId: id, name: "" })),
      categories: selectedCategories.map((id: number) => ({ categoryId: id, name: "" })),
      subCategories: selectedSubCategories.map((id: number) => ({ subCategoryId: id, categoryId: id, name: "" })),
      publicationYear: this.publicationYear?.value === '' ? null : this.publicationYear?.value as number
    };

    // realizar solicitud a la api
    this.bookService.getFilteredBook(this.filterBookDto).subscribe({
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
        this.dataSource.data = response.result as BookDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })

  }

  // Obtener libros de la api
  private getBooksDto(): void {
    this.bookService.getAll().subscribe({
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
        this.dataSource.data = response.result as BookDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000,
          easeTime: 1000
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

  // Obtener categorias de la api
  private getCategoriesDto(): void {
    this.categoryService.getAll().subscribe({
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
        this.categories = list as CategoryDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  // Obtener sub categorias de l api
  private getSubCategoriesDto(): void {
    this.subCategoryService.getAll().subscribe({
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
        this.subCategories = response.result as SubCategoryDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  // Obtener editoriales de la api
  private getPublishersDto(): void {
    this.publisherService.getAll().subscribe({
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
        this.publishers = list as PublisherDto[];
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    });
  }

  /* Getters */
  get categoryIds() {
    return this.filterForm.get('categoryIds');
  }

  get authorIds() {
    return this.filterForm.get('authorIds');
  }

  get publisherIds() {
    return this.filterForm.get('publisherIds');
  }

  get subCategoryIds() {
    return this.filterForm.get('subCategoryIds');
  }

  get publicationYear() {
    return this.filterForm.get('publicationYear');
  }

}
