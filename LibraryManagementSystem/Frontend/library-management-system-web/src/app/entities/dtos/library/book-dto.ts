import { PublisherDto } from "./publisher-dto";
import { CategoryDto } from "./category-dto";
import { AuthorDto } from "./author-dto";
import { SubCategoryDto } from "./sub-category-dto";

export interface BookDto {
    bookId: number;
    iSBN10: string | null;
    iSBN13: string | null;
    classification: string | null;
    title: string | null;
    description: string | null;
    publicationYear: number;
    image: string | null;
    isActive: boolean;
    numberOfCopies: number;
    borrowedCopies: number;
    isAvailable: boolean;
    publisher: PublisherDto | null;
    category: CategoryDto | null;
    authors: AuthorDto[] | null;
    subCategories: SubCategoryDto[] | null;
}