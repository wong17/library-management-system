import { CareerDto } from "./career-dto";

export interface StudentDto {
    studentId: number;
    firstName: string | null;
    secondName: string | null;
    firstLastname: string | null;
    secondLastname: string | null;
    carnet: string | null;
    phoneNumber: string | null;
    sex: string;
    email: string | null;
    shift: string | null;
    borrowedBooks: number;
    hasBorrowedMonograph: boolean;
    fine: number;
    createdOn: Date;
    modifiedOn: Date;
    career: CareerDto | null;
}