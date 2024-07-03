import { BookDto } from "./book-dto";
import { LoanDto } from "./loan-dto";

export interface BookLoanDto extends LoanDto {
    bookLoanId: number;
    typeOfLoan: string | null;
    book: BookDto | null;
}