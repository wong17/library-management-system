import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable } from 'rxjs';
import { MonographInsertDto } from '../../entities/dtos/library/monograph-insert-dto';
import { MonographUpdateDto } from '../../entities/dtos/library/monograph-update-dto';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class MonographService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Monograph urls */
  private readonly monographCreateUrl: string = '/api/monographs/create'
  private readonly monographUpdateUrl: string = '/api/monographs/update'
  private readonly monographDeleteUrl: string = '/api/monographs/delete'
  private readonly monographGetAllUrl: string = '/api/monographs/get_all'
  private readonly monographGetByIdUrl: string = '/api/monographs/get_by_id'

  /* Encabezado http para solicitudes POST y PUT */
  private readonly httpHeader = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: 'application/json, text/plain, */*'
    })
  }

  /**
   * Modulo HttpClient para enviar solicitudes http
   * @param http
   */
  constructor(private readonly http: HttpClient) { }

  /**
   * Inserta una nueva monografía 
   * @param monograph
   * @returns Observable<ApiResponse>
   */
  create(monograph: MonographInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.monographCreateUrl}`, monograph, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza una monografía
   * @param monograph
   * @returns Observable<ApiResponse>
   */
  update(monograph: MonographUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.monographUpdateUrl}`, monograph, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina una monografía
   * @param monographId
   * @returns Observable<ApiResponse>
   */
  delete(monographId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.monographDeleteUrl}/${monographId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todas las monografías
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographGetAllUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve una monografía
   * @param monographId
   * @returns Observable<ApiResponse>
   */
  getById(monographId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographGetByIdUrl}/${monographId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
