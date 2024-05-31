import { CareerDto } from "../university/career-dto";
import { AuthorDto } from "./author-dto";

export interface MonographDto {
    MonographId: number;
    Classification: string | null;
    Title: string | null;
    Description: string | null;
    Tutor: string | null;
    PresentationDate: Date;
    Image: string | null;
    IsActive: boolean;
    IsAvailable: boolean;
    Career: CareerDto | null;
    Authors: AuthorDto[] | null;
}