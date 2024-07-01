import { AuthorDto } from "./author-dto";
import { CategoryDto } from "./category-dto";
import { PublisherDto } from "./publisher-dto";
import { SubCategoryFilterDto } from "./sub-category-filter-dto";

export interface FilterBookDto {
    authors: AuthorDto[] | null;
    publishers: PublisherDto[] | null;
    categories: CategoryDto[] | null;
    subCategories: SubCategoryFilterDto[] | null;
    publicationYear: number | null;
}