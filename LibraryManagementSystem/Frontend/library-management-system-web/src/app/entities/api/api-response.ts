export interface ApiResponse {
    IsSuccess: number;
    StatusCode: number;
    Message: string | null;
    Result: any | null;
}