export interface BookLoanInsertDto {
    studentId: number;
    bookId: number;
    typeOfLoan: string | null;
}