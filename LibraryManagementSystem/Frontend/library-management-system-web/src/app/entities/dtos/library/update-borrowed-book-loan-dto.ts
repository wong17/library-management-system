export interface UpdateBorrowedBookLoanDto {
    bookLoanId: number;
    dueDate: Date;
    borrowedUserId: number;
}