export interface MonographInsertDto {
    classification: string | null;
    title: string | null;
    description: string | null;
    tutor: string | null;
    presentationDate: Date;
    image: string | null;
    careerId: number;
    isActive: boolean;
}