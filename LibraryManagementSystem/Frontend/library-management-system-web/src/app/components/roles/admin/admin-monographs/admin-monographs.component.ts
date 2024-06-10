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

@Component({
  selector: 'app-admin-monographs',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule],
  templateUrl: './admin-monographs.component.html',
  styleUrl: './admin-monographs.component.css'
})
export class AdminMonographsComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['monographId', 'classification', 'title', 'description', 'tutor', 'presentationDate', 'isActive', 'isAvailable', 'careerName', 'authors'];

  /*  */
  dataSource: MatTableDataSource<MonographDto> = new MatTableDataSource<MonographDto>();
  /* Obtener el objeto de paginado */
  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  /* Obtener el objeto de ordenamiento */
  @ViewChild(MatSort) sort: MatSort | null = null;

  constructor(private monographService: MonographService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getMonographsDto();
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

  insertNewMonographBtn() {

  }

}
