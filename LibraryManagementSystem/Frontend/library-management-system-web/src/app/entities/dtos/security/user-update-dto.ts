export interface UserUpdateDto {
    userId: number;
    email: string | null;
    password: string | null;
}