import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable } from 'rxjs';
import { MonographAuthorInsertDto } from '../../entities/dtos/library/monograph-author-insert-dto';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class MonographAuthorService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* MonographAuthor urls */
  private readonly monographAuthorCreateUrl: string = '/api/MonographAuthor/Create'
  private readonly monographAuthorCreateManyUrl: string = '/api/MonographAuthor/CreateMany'
  private readonly monographAuthorUpdateManyUrl: string = '/api/MonographAuthor/UpdateMany'
  private readonly monographAuthorUrl: string = '/api/MonographAuthor'

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
   * Inserta un autor a una monografía
   * @param monographAuthor
   * @returns Observable<ApiResponse>
   */
  create(monographAuthor: MonographAuthorInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.monographAuthorCreateUrl}`, monographAuthor, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Inserta varios autores a una monografía
   * @param monographAuthors
   * @returns Observable<ApiResponse>
   */
  createMany(monographAuthors: MonographAuthorInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.monographAuthorCreateManyUrl}`, monographAuthors, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza varios autores de una monografía
   * @param monographAuthors
   * @returns Observable<ApiResponse>
   */
  updateMany(monographAuthors: MonographAuthorInsertDto[]): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.monographAuthorUpdateManyUrl}`, monographAuthors, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina un autor de la monografía
   * @param (monographId authorId)
   * @returns Observable<ApiResponse>
   */
  delete(monographId: number, authorId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.monographAuthorUrl}/${monographId}/${authorId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todos los autores de todas las monografías
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographAuthorUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un autor de la monografía
   * @param (monographId authorId)
   * @returns Observable<ApiResponse>
   */
  getById(monographId: number, authorId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographAuthorUrl}/${monographId}/${authorId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
