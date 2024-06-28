import { RoleDto } from "./role-dto";

export interface UserDto {
    userId: number;
    userName: string | null;
    email: string | null;
    active: boolean;
    accessToken: string | null;
    refreshToken: string | null;
    refreshTokenExpiryTime: Date;
    lockoutEnabled: boolean;
    accessFailedCount: number;
    roles: RoleDto[] | null;
}