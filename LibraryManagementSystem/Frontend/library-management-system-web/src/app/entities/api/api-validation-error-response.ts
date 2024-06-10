export interface ApiValidationErrorResponse {
    type: string;
    title: string;
    status: number;
    errors: {
        [fieldName: string]: string[];
    };
    traceId: string;
}
