import { StudentDto } from "../university/student-dto";
import { MonographDto } from "./monograph-dto";

export interface MonographLoanDto {
    MonographLoanId: number;
    State: string | null;
    LoanDate: Date;
    DueDate: Date;
    ReturnDate: Date;
    Student: StudentDto | null;
    Monograph: MonographDto | null;
}