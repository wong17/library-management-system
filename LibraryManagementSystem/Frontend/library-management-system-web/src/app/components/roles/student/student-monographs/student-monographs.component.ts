import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MonographDto } from '../../../../entities/dtos/library/monograph-dto';
import { ApiResponse } from '../../../../entities/api/api-response';
import { MonographService } from '../../../../services/library/monograph.service';
import { ToastrService } from 'ngx-toastr';
import { MatSort } from '@angular/material/sort';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { LoanDialogData, TypeOfLoan } from '../../../../util/dialog-data';
import { StudentMonographLoansDialogComponent } from '../student-monograph-loans-dialog/student-monograph-loans-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatChipsModule } from '@angular/material/chips';
import { MatCheckbox } from '@angular/material/checkbox';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-student-monographs',
  standalone: true,
  imports: [MatTableModule, MatInputModule, MatFormFieldModule, MatPaginator, MatPaginatorModule, MatButtonModule, MatIconModule, 
    MatCheckbox, MatChipsModule, MatTooltipModule],
  templateUrl: './student-monographs.component.html',
  styleUrl: './student-monographs.component.css'
})
export class StudentMonographsComponent implements OnInit, AfterViewInit {

  displayedColumns: string[] = ['image', 'classification', 'title', 'description', 'tutor', 'presentationDate', 'careerName', 'authors', 'options'];

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

  requestMonographLibraryRoomClick(monograph: MonographDto) {
    // data
    const loanDialogData: LoanDialogData = {
      title: 'Préstamo de monografía en sala',
      typeOfLoan: TypeOfLoan.Library,
      data: monograph
    };
    // Abrir el dialogo y obtener una referencia de el
    this.dialog.open(StudentMonographLoansDialogComponent, {
      width: '800px',
      disableClose: true,
      data: loanDialogData
    });
  }

}
