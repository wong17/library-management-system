import { StudentDto } from "../university/student-dto";
import { BookDto } from "./book-dto";

export interface BookLoanDto {
    BookLoanId: number;
    TypeOfLoan: string | null;
    State: string | null;
    LoanDate: Date;
    DueDate: Date;
    ReturnDate: Date;
    Student: StudentDto | null;
    Book: BookDto | null;
}