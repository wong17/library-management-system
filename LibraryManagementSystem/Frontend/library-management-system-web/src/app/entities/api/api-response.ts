export interface ApiResponse {
    isSuccess: number;
    statusCode: number;
    message: string | null;
    result: any | null;
}