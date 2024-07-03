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
import { ApiResponse } from '../../../../entities/api/api-response';
import { AdminBooksDialogComponent } from '../admin-books-dialog/admin-books-dialog.component';
import { AdminBooksSubCategoriesDialogComponent } from '../admin-books-sub-categories-dialog/admin-books-sub-categories-dialog.component';
import { AdminBooksAuthorsDialogComponent } from '../admin-books-authors-dialog/admin-books-authors-dialog.component';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';
import { AuthorService } from '../../../../services/library/author.service';
import { PublisherService } from '../../../../services/library/publisher.service';
import { CategoryService } from '../../../../services/library/category.service';
import { SubCategoryService } from '../../../../services/library/sub-category.service';
import { AuthorDto } from '../../../../entities/dtos/library/author-dto';
import { CategoryDto } from '../../../../entities/dtos/library/category-dto';
import { SubCategoryDto } from '../../../../entities/dtos/library/sub-category-dto';
import { PublisherDto } from '../../../../entities/dtos/library/publisher-dto';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FilterBookDto } from '../../../../entities/dtos/library/filter-book-dto';
import { StringUtil } from '../../../../util/string-util';
import { BookSignalRService } from '../../../../services/signalr-hubs/book-signal-r.service';
import { Year } from '../../../../util/year';

@Component({
  selector: 'app-admin-books',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule,
    MatCheckbox, MatChipsModule, MatTooltipModule, MatSelectModule, ReactiveFormsModule],
  templateUrl: './admin-books.component.html',
  styleUrl: './admin-books.component.css'
})
export class AdminBooksComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['bookId', 'image', 'isbN10', 'isbN13', 'classification', 'title', 'description', 'publicationYear', 'numberOfCopies',
    'borrowedCopies', 'publisherName', 'categoryName', 'authors', 'subCategories', 'isAvailable', 'isActive', 'options'];

  /* */
  filterForm: FormGroup;
  /* Categorias */
  categories: CategoryDto[] | undefined;
  /* Editoriales */
  publishers: PublisherDto[] | undefined;
  /* Autores */
  authors: AuthorDto[] | undefined;
  /* Sub categorias */
  subCategories: SubCategoryDto[] | undefined;
  /* Años que se publicaron los libros */
  availableYears: Year[] = [];

  filterBookDto: FilterBookDto = { authors: null, categories: null, publishers: null, subCategories: null, publicationYear: 0 };

  /*  */
  dataSource: MatTableDataSource<BookDto> = new MatTableDataSource<BookDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;

  constructor(
    private bookService: BookService,
    private authorService: AuthorService,
    private publisherService: PublisherService,
    private categoryService: CategoryService,
    private subCategoryService: SubCategoryService,
    private dialog: MatDialog,
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
    //
    this.availableYears.push({ year: null, text: 'Todos' })
    //
    const currentYear = new Date().getFullYear();
    for (let year = currentYear; year >= 1900; year--) {
      this.availableYears.push({ text: `${year}`, year: year });
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

          this.getBooksDto();
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

  editBookAuthorsClick(element: BookDto) {
    // data
    const dialogData: DialogData = {
      title: element.authors?.length === 0 ? `Agregar autores de ${element.title}` : `Editar autores de ${element.title}`,
      operation: element.authors?.length === 0 ? DialogOperation.Add : DialogOperation.Update,
      data: element
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminBooksAuthorsDialogComponent, {
      width: '500px',
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

  editBookSubCategoriesClick(element: BookDto) {
    // data
    const dialogData: DialogData = {
      title: element.subCategories?.length === 0 ? `Agregar sub categorías a ${element.title}` : `Editar sub categorías a ${element.title}`,
      operation: element.subCategories?.length === 0 ? DialogOperation.Add : DialogOperation.Update,
      data: element
    };
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminBooksSubCategoriesDialogComponent, {
      width: '500px',
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
      publicationYear: (!this.publicationYear?.value || this.publicationYear?.value === 0 || this.publicationYear?.value === '') 
                        ? null : this.publicationYear?.value as number
    };
    debugger;
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
