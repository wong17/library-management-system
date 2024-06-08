export interface BookInsertDto {
    iSbN10: string | null;
    iSbN13: string | null;
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