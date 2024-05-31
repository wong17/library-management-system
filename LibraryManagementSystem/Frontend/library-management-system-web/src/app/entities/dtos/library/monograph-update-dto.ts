export interface MonographUpdateDto {
    monographId: number;
    classification: string | null;
    title: string | null;
    description: string | null;
    tutor: string | null;
    presentationDate: Date;
    image: string | null;
    careerId: number;
    isAvailable: boolean;
    isActive: boolean;
}