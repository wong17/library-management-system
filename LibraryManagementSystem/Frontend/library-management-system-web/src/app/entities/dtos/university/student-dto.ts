import { CareerDto } from "./career-dto";

export interface StudentDto {
    StudentId: number;
    FirstName: string | null;
    SecondName: string | null;
    FirstLastname: string | null;
    SecondLastname: string | null;
    Carnet: string | null;
    PhoneNumber: string | null;
    Sex: string;
    Email: string | null;
    Shift: string | null;
    BorrowedBooks: number;
    HasBorrowedMonograph: boolean;
    Fine: number;
    CreatedOn: Date;
    ModifiedOn: Date;
    Career: CareerDto | null;
}