export interface BookInsertDto {
    isbN10: string | null;
    isbN13: string | null;
    classification: string | null;
    title: string | null;
    description: string | null;
    publicationYear: number;
    image: string | null;
    publisherId: number;
    categoryId: number;
    numberOfCopies: number;
    isActive: boolean;
}