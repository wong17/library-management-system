export interface BookUpdateDto {
    bookId: number;
    iSBN10: string | null;
    iSBN13: string | null;
    classification: string | null;
    title: string | null;
    description: string | null;
    publicationYear: number;
    image: string | null;
    publisherId: number;
    categoryId: number;
    numberOfCopies: number;
    isAvailable: boolean;
}