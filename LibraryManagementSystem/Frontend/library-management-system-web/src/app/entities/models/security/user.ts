export class User {
    userId: number | undefined;
    userName: string | null | undefined;
    email: string | null | undefined;
    password: string |null | undefined;
    active: boolean | null | undefined;
    accessToken: string | null | undefined;
    refreshToken: string | null | undefined;
    refreshTokenExpiryTime: Date | undefined;
    lockoutEnabled: boolean | undefined;
    accessFailedCount: number | undefined;
}
