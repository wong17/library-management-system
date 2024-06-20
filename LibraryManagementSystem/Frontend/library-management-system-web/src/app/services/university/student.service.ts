import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable } from 'rxjs';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Student urls */
  private readonly studentUrl: string = '/api/Student'

  /**
   * Modulo HttpClient para enviar solicitudes http
   * @param http
   */
  constructor(private readonly http: HttpClient) { }

  /**
   * Devuelve todos los estudiantes
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.studentUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un estudiante por su id
   * @param studentId
   * @returns Observable<ApiResponse>
   */
  getById(studentId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.studentUrl}/${studentId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un estudiante por su carnet
   * @param carnet
   * @returns Observable<ApiResponse>
   */
  getByCarnet(carnet: string): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.studentUrl}/${carnet}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
