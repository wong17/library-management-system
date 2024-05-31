export interface UserDto {
    UserId: number;
    UserName: string | null;
    Email: string | null;
    Password: string | null;
    AccessToken: string | null;
    RefreshToken: string | null;
    RefreshTokenExpiryTime: Date;
    LockoutEnabled: boolean;
    AccessFailedCount: number;
}