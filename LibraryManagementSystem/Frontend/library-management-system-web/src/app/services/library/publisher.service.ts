import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable } from 'rxjs';
import { PublisherInsertDto } from '../../entities/dtos/library/publisher-insert-dto';
import { PublisherUpdateDto } from '../../entities/dtos/library/publisher-update-dto';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class PublisherService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Publisher urls */
  private readonly publisherCreateUrl: string = '/api/publishers/create'
  private readonly publisherCreateManyUrl: string = '/api/publishers/create_many'
  private readonly publisherUpdateUrl: string = '/api/publishers/update'
  private readonly publisherUpdateManyUrl: string = '/api/publishers/update_many'
  private readonly publisherDeleteUrl: string = '/api/publishers/delete'
  private readonly publisherGetAllUrl: string = '/api/publishers/get_all'
  private readonly publisherGetByIdUrl: string = '/api/publishers/get_by_id'

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
   * Inserta una editorial
   * @param publisher
   * @returns Observable<ApiResponse>
   */
  create(publisher: PublisherInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.publisherCreateUrl}`, publisher, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Inserta varias editoriales
   * @param publishers
   * @returns Observable<ApiResponse>
   */
  createMany(publishers: PublisherInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.publisherCreateManyUrl}`, publishers, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza una editorial
   * @param publisher
   * @returns Observable<ApiResponse>
   */
  update(publisher: PublisherUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.publisherUpdateUrl}`, publisher, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza varias editoriales
   * @param publishers
   * @returns Observable<ApiResponse>
   */
  updateMany(publishers: PublisherUpdateDto[]): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.publisherUpdateManyUrl}`, publishers, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina una editorial
   * @param publisherId
   * @returns Observable<ApiResponse>
   */
  delete(publisherId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.publisherDeleteUrl}/${publisherId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todas las editoriales
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.publisherGetAllUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve una editorial
   * @param publisherId
   * @returns Observable<ApiResponse>
   */
  getById(publisherId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.publisherGetByIdUrl}/${publisherId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
