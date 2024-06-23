import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ControlStateMatcher } from '../../../../util/control-state-matcher';
import { MonographLoanInsertDto } from '../../../../entities/dtos/library/monograph-loan-insert-dto';
import { StudentDto } from '../../../../entities/dtos/university/student-dto';
import { MonographDto } from '../../../../entities/dtos/library/monograph-dto';
import { LoanDialogData } from '../../../../util/dialog-data';
import { MonographLoanService } from '../../../../services/library/monograph-loan.service';
import { StudentService } from '../../../../services/university/student.service';
import { ToastrService } from 'ngx-toastr';
import { ApiResponse } from '../../../../entities/api/api-response';

@Component({
  selector: 'app-student-monograph-loans-dialog',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, MatInputModule, MatIconModule, MatFormFieldModule, ReactiveFormsModule, MatSelectModule],
  templateUrl: './student-monograph-loans-dialog.component.html',
  styleUrl: './student-monograph-loans-dialog.component.css'
})
export class StudentMonographLoansDialogComponent {

  /* Referencia del formulario */
  studentMonographLoanForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()
  /* Dtos */
  monographLoanInsertDto: MonographLoanInsertDto = { monographid: 0, studentId: 0 }
  studentDto: StudentDto | undefined
  monographDto: MonographDto | undefined

  constructor(
    public dialogRef: MatDialogRef<StudentMonographLoansDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public loanDialogData: LoanDialogData,
    private monographLoanService: MonographLoanService,
    private studentService: StudentService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.studentMonographLoanForm = this.formBuilder.group({
      carnet: ['', [Validators.required, Validators.minLength(10)]],
      fullName: [''],
      career: [''],
      sex: [''],
      shift: [''],
      classification: [''],
      title: [''],
      authors: ['']
    });

    if (loanDialogData.data) {
      this.monographDto = this.loanDialogData.data as MonographDto

      this.studentMonographLoanForm.patchValue({
        classification: this.monographDto.classification,
        title: this.monographDto.title, authors: this.monographDto.authors?.toString()
      })

      this.monographLoanInsertDto = {
        monographid: this.monographDto.monographId,
        studentId: 0 // default
      }

    }
  }

  /* Guardar */
  onSubmit() {
    // save 
    this.save();
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
    this.monographLoanInsertDto.studentId = this.studentDto.studentId
    // Realizar solicitud http
    this.monographLoanService.create(this.monographLoanInsertDto).subscribe({
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
        this.studentMonographLoanForm.patchValue({
          fullName: `${this.studentDto.firstName} ${this.studentDto.secondName} ${this.studentDto.firstLastname} ${this.studentDto.secondLastname}`,
          career: `${this.studentDto.career?.name}`,
          sex: `${this.studentDto.sex}`,
          shift: `${this.studentDto.shift}`
        })
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
    return this.studentMonographLoanForm.get('carnet');
  }

  get typeOfLoan() {
    return this.studentMonographLoanForm.get('typeOfLoan');
  }

  get fullName() {
    return this.studentMonographLoanForm.get('fullName');
  }

  get career() {
    return this.studentMonographLoanForm.get('career');
  }

  get sex() {
    return this.studentMonographLoanForm.get('sex');
  }

  get shift() {
    return this.studentMonographLoanForm.get('shift');
  }

  get classification() {
    return this.studentMonographLoanForm.get('classification');
  }

  get title() {
    return this.studentMonographLoanForm.get('title');
  }

  get authors() {
    return this.studentMonographLoanForm.get('authors');
  }

}
