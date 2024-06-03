import { StudentDto } from "../university/student-dto";
import { BookDto } from "./book-dto";

export interface BookLoanDto {
    bookLoanId: number;
    typeOfLoan: string | null;
    state: string | null;
    loanDate: Date;
    dueDate: Date;
    returnDate: Date;
    student: StudentDto | null;
    book: BookDto | null;
}