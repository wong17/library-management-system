import { CareerDto } from "../university/career-dto";
import { AuthorDto } from "./author-dto";

export interface MonographDto {
    monographId: number;
    classification: string | null;
    title: string | null;
    description: string | null;
    tutor: string | null;
    presentationDate: Date;
    image: string | null;
    isActive: boolean;
    isAvailable: boolean;
    career: CareerDto | null;
    authors: AuthorDto[] | null;
}