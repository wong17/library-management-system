import { CategoryDto } from "./category-dto";

export interface SubCategoryDto {
    subCategoryId: number;
    category: CategoryDto | null;
    name: string | null;
}