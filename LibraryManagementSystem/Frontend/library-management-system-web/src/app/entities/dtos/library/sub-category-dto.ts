import { CategoryDto } from "./category-dto";

export interface SubCategoryDto {
    SubCategoryId: number;
    Category: CategoryDto | null;
    Name: string | null;
}