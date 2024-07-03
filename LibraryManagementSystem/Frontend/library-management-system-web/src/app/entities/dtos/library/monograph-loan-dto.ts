import { LoanDto } from "./loan-dto";
import { MonographDto } from "./monograph-dto";

export interface MonographLoanDto extends LoanDto {
    monographLoanId: number;
    monograph: MonographDto | null;
}