import { UserDto } from "../security/user-dto";
import { StudentDto } from "../university/student-dto";
import { MonographDto } from "./monograph-dto";

export interface MonographLoanDto {
    monographLoanId: number;
    state: string | null;
    loanDate: Date;
    dueDate: Date;
    returnDate: Date;
    student: StudentDto | null;
    monograph: MonographDto | null;
    borrowedUser: UserDto | null;
    returnedUser: UserDto | null;
}