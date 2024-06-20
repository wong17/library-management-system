import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { ApiResponse } from '../../entities/api/api-response';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class CareerService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Career urls */
  private readonly careerGetAllUrl: string = '/api/careers/get_all'
  private readonly careerGetByIdUrl: string = '/api/careers/get_by_id'

  /**
   * Modulo HttpClient para enviar solicitudes http
   * @param http
   */
  constructor(private readonly http: HttpClient) { }

  /**
   * Devuelve todas las carreras
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.careerGetAllUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve una carrera
   * @param careerId
   * @returns Observable<ApiResponse>
   */
  getById(careerId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.careerGetByIdUrl}/${careerId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
