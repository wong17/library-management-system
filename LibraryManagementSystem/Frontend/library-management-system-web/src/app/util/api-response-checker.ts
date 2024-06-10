import { ApiResponse } from "../entities/api/api-response";

export class ApiResponseChecker {

    public static isApiResponse(obj: any): obj is ApiResponse {
        return typeof obj === 'object' &&
            'isSuccess' in obj &&
            'statusCode' in obj &&
            'message' in obj &&
            'result' in obj;
    }
}