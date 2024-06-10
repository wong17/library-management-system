export interface BookUpdateDto {
    bookId: number;
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