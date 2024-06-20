import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CategoryInsertDto } from '../../entities/dtos/library/category-insert-dto';
import { catchError, Observable } from 'rxjs';
import { ApiResponse } from '../../entities/api/api-response';
import { CategoryUpdateDto } from '../../entities/dtos/library/category-update-dto';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Category urls */
  private readonly categoryCreateUrl: string = '/api/categories/create'
  private readonly categoryCreateManyUrl: string = '/api/categories/create_many'
  private readonly categoryUpdateUrl: string = '/api/categories/update'
  private readonly categoryUpdateManyUrl: string = '/api/categories/update_many'
  private readonly categoryDeleteUrl: string = '/api/categories/delete'
  private readonly categoryGetAllUrl: string = '/api/categories/get_all'
  private readonly categoryGetByIdUrl: string = '/api/categories/get_by_id'

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
   * Inserta una categoria
   * @param category
   * @returns Observable<ApiResponse>
   */
  create(category: CategoryInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.categoryCreateUrl}`, category, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Inserta varias categorias
   * @param categories
   * @returns Observable<ApiResponse>
   */
  createMany(categories: CategoryInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.categoryCreateManyUrl}`, categories, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza un categoria
   * @param category
   * @returns Observable<ApiResponse>
   */
  update(category: CategoryUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.categoryUpdateUrl}`, category, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza varias categorias
   * @param categories
   * @returns Observable<ApiResponse>
   */
  updateMany(categories: CategoryUpdateDto[]): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.categoryUpdateManyUrl}`, categories, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina una categoria
   * @param categoryId
   * @returns Observable<ApiResponse>
   */
  delete(categoryId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.categoryDeleteUrl}/${categoryId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todas las categorias
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.categoryGetAllUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve una categoria
   * @param categoryId
   * @returns Observable<ApiResponse>
   */
  getById(categoryId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.categoryGetByIdUrl}/${categoryId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
