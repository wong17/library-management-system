import { PublisherDto } from "./publisher-dto";
import { CategoryDto } from "./category-dto";
import { AuthorDto } from "./author-dto";
import { SubCategoryDto } from "./sub-category-dto";

export interface BookDto {
    BookId: number;
    ISBN10: string | null;
    ISBN13: string | null;
    Classification: string | null;
    Title: string | null;
    Description: string | null;
    PublicationYear: number;
    Image: string | null;
    IsActive: boolean;
    NumberOfCopies: number;
    BorrowedCopies: number;
    IsAvailable: boolean;
    Publisher: PublisherDto | null;
    Category: CategoryDto | null;
    Authors: AuthorDto[] | null;
    SubCategories: SubCategoryDto[] | null;
}