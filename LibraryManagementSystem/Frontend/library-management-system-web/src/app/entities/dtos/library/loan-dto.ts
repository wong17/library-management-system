import { UserDto } from "../security/user-dto";
import { StudentDto } from "../university/student-dto";

export interface LoanDto {
    state: string | null;
    loanDate: Date;
    dueDate: Date;
    returnDate: Date;
    student: StudentDto | null;
    borrowedUser: UserDto | null;
    returnedUser: UserDto | null;
}