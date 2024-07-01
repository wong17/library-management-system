import { CareerDto } from "../university/career-dto";
import { AuthorDto } from "./author-dto";

export interface FilterMonographDto {
    authors: AuthorDto[] | null;
    careers: CareerDto[] | null;
    beginPresentationDate: Date | null;
    endPresentationDate: Date | null;
}