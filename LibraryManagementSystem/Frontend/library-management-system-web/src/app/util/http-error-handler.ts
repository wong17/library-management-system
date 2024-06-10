import { Observable, of } from "rxjs";
import { ApiResponse } from "../entities/api/api-response";
import { HttpErrorResponse } from "@angular/common/http";
import { ApiResponseChecker } from "./api-response-checker";
import { ApiValidationErrorResponse } from "../entities/api/api-validation-error-response";

export class HttpErrorHandler {

    public static handlerError(error: HttpErrorResponse): Observable<ApiResponse> {
        
        // Cuando sea null o undefined
        if (!error) {
            const apiResponse: ApiResponse = {
                isSuccess: 1,
                statusCode: 400,
                result: null,
                message: 'HttpErrorResponse es null|undefined'
            }
            return of(apiResponse)
        }        

        // Cuando el error viene desde la base de datos
        if (ApiResponseChecker.isApiResponse(error.error)) {
            return of(error.error);
        }

        // Cuando lo lanza el ModelState al validar el Dto
        const validationResponse = error.error as ApiValidationErrorResponse;
        const firstErrorKey = Object.keys(validationResponse.errors)[0];
        const firstError = validationResponse.errors[firstErrorKey][0];

        const apiResponse: ApiResponse = {
            isSuccess: 1,
            statusCode: validationResponse.status,
            result: null,
            message: firstError
        };
        return of(apiResponse);
    }

}