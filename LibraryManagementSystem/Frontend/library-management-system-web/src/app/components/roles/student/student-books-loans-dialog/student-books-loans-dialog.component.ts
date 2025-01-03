import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ControlStateMatcher } from '../../../../util/control-state-matcher';
import { BookLoanInsertDto } from '../../../../entities/dtos/library/book-loan-insert-dto';
import { ToastrService } from 'ngx-toastr';
import { LoanDialogData, TypeOfLoan } from '../../../../util/dialog-data';
import { BookLoanService } from '../../../../services/library/book-loan.service';
import { ApiResponse } from '../../../../entities/api/api-response';
import { StudentDto } from '../../../../entities/dtos/university/student-dto';
import { StudentService } from '../../../../services/university/student.service';
import { BookDto } from '../../../../entities/dtos/library/book-dto';

@Component({
  selector: 'app-student-books-loans-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatInputModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './student-books-loans-dialog.component.html',
  styleUrl: './student-books-loans-dialog.component.css'
})
export class StudentBooksLoansDialogComponent {

  /* Referencia del formulario */
  studentForm: FormGroup;
  studentBookLoanForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()
  /* Dtos */
  bookLoanInsertDto: BookLoanInsertDto = { bookId: 0, studentId: 0, typeOfLoan: '' }
  studentDto: StudentDto | undefined
  bookDto: BookDto | undefined

  isFormFilled: boolean = false

  constructor(
    public dialogRef: MatDialogRef<StudentBooksLoansDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public loanDialogData: LoanDialogData,
    private bookLoanService: BookLoanService,
    private studentService: StudentService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.studentForm = this.formBuilder.group({
      carnet: ['', [Validators.required, Validators.minLength(10)]],
      typeOfLoan: ['', [Validators.required]]
    })

    this.studentBookLoanForm = this.formBuilder.group({
      fullName: [''],
      career: [''],
      sex: [''],
      shift: [''],
      classification: [''],
      title: [''],
      publicationYear: ['']
    });

    if (loanDialogData.data) {
      this.bookDto = this.loanDialogData.data as BookDto

      this.studentForm.patchValue({
        typeOfLoan: loanDialogData.typeOfLoan === TypeOfLoan.Home ? 'DOMICILIO' : 'SALA'
      })

      this.studentBookLoanForm.patchValue({
        classification: this.bookDto.classification,
        title: this.bookDto.title,
        publicationYear: this.bookDto.publicationYear
      })

      this.bookLoanInsertDto = {
        bookId: this.bookDto.bookId,
        typeOfLoan: loanDialogData.typeOfLoan === TypeOfLoan.Home ? 'DOMICILIO' : 'SALA',
        studentId: 0 // default
      }

    }
  }

  clearForm() {
    this.studentForm.get('carnet')?.reset();
    this.studentBookLoanForm.patchValue({
      fullName: '',
      career: '',
      sex: '',
      shift: ''
    });
    this.studentDto = undefined;
    this.isFormFilled = false;
  }

  /* Guardar o actualizar */
  onSubmit() {
    // save 
    this.save()
  }

  /* Guardar */
  private save(): void {
    // Obtener informacion del estudiante
    if (!this.studentDto?.studentId) {
      this.toastr.error(`Estudiante es obligatorio`, 'Error', {
        timeOut: 5000
      });
      return;
    }
    // Obtener id del estudiante
    this.bookLoanInsertDto.studentId = this.studentDto.studentId
    // Realizar solicitud http
    this.bookLoanService.create(this.bookLoanInsertDto).subscribe({
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

        this.closeDialog(true);
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  searchStudent() {
    // Validar que se haya ingresdo un carnet
    if (!this.isFormFilled && !this.carnet?.value) return;

    // Obtener informacion de los campos
    if (!this.carnet?.value) {
      this.toastr.error(`Carnet del estudiante es obligatorio`, 'Error', {
        timeOut: 5000
      })
      return
    }
    //
    this.studentService.getByCarnet(this.carnet?.value).subscribe({
      next: response => {
        // Ocurrio un error
        if (response.isSuccess !== 0 || response.statusCode !== 200) {
          this.toastr.warning(`${response.message}`, 'Atención', {
            timeOut: 5000
          })
          return;
        }
        // 
        const student = response.result as StudentDto;
        if (!student)
          return;

        this.studentDto = student;
        this.studentBookLoanForm.patchValue({
          fullName: `${this.studentDto.firstName} ${this.studentDto.secondName} ${this.studentDto.firstLastname} ${this.studentDto.secondLastname}`,
          career: `${this.studentDto.career?.name}`,
          sex: `${this.studentDto.sex}`,
          shift: `${this.studentDto.shift}`
        })
        this.isFormFilled = true;
      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  /* */
  closeDialog(done: boolean): void {
    this.dialogRef.close(done);
  }

  /* Getters */
  get carnet() {
    return this.studentForm.get('carnet');
  }

  get typeOfLoan() {
    return this.studentForm.get('typeOfLoan');
  }

  get fullName() {
    return this.studentBookLoanForm.get('fullName');
  }

  get career() {
    return this.studentBookLoanForm.get('career');
  }

  get sex() {
    return this.studentBookLoanForm.get('sex');
  }

  get shift() {
    return this.studentBookLoanForm.get('shift');
  }

  get classification() {
    return this.studentBookLoanForm.get('classification');
  }

  get title() {
    return this.studentBookLoanForm.get('title');
  }

  get publicationYear() {
    return this.studentBookLoanForm.get('publicationYear');
  }

}
