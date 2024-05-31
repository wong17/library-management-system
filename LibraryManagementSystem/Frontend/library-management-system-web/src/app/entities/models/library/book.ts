export class Book {
    bookId: number | undefined;
    iSBN10: string | null | undefined;
    iSBN13: string | null | undefined;
    classification: string | null | undefined;
    title: string | null | undefined;
    description: string | null | undefined;
    publicationYear: number | undefined;
    image: string | null | undefined;
    isActive: boolean | undefined;
    publisherId: number | undefined;
    categoryId: number | undefined;
    numberOfCopies: number | undefined;
    borrowedCopies: number | undefined;
    isAvailable: boolean | undefined;
}
