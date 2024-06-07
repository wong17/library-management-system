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

@Component({
  selector: 'app-admin-publishers',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule],
  templateUrl: './admin-publishers.component.html',
  styleUrl: './admin-publishers.component.css'
})
export class AdminPublishersComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['id', 'name'];

  /*  */
  dataSource: MatTableDataSource<PublisherDto> = new MatTableDataSource<PublisherDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;
  /* Editoriales */
  publishers: PublisherDto[] | undefined;

  constructor(private publisherService: PublisherService, private dialog: MatDialog) { }

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

  insertNewPublisherBtn() {
    // Abrir el dialogo y obtener una referencia de el
    const dialogRef = this.dialog.open(AdminPublishersDialogComponent, {
      width: '500px',
      disableClose: true,
      data: ['Nueva Editorial', true]
    });
    // Refrescar tabla despúes que el dialogo se cierre si se agrego un nuevo registro
    dialogRef.afterClosed().subscribe((done) => {
      if (done)
        this.getPublishersDto();
    });
  }

  private getPublishersDto(): void {
    this.publisherService.getAll().subscribe({
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
        const list: PublisherDto[] = response.result as PublisherDto[];
        this.dataSource.data = list;
      },
      error: error => {
        console.error(error);
      }
    })
  }

  
  
}
