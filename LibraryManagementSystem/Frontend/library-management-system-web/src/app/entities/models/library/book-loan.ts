export class BookLoan {
    bookLoanId: number | undefined;
    studentId: number | undefined;
    bookId: number | undefined;
    typeOfLoan: string | null | undefined;
    state: string | null | undefined;
    loanDate: Date | undefined;
    dueDate: Date | undefined;
    returnDate: Date | undefined;
}
