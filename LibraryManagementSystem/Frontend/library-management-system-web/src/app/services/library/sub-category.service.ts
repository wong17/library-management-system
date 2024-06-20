import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable } from 'rxjs';
import { SubCategoryInsertDto } from '../../entities/dtos/library/sub-category-insert-dto';
import { SubCategoryUpdateDto } from '../../entities/dtos/library/sub-category-update-dto';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class SubCategoryService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* SubCategory urls */
  private readonly subCategoryCreateUrl: string = '/api/subcategories/create'
  private readonly subCategoryCreateManyUrl: string = '/api/subcategories/create_many'
  private readonly subCategoryUpdateUrl: string = '/api/subcategories/update'
  private readonly subCategoryUpdateManyUrl: string = '/api/subcategories/update_many'
  private readonly subCategoryDeleteUrl: string = '/api/subcategories/delete'
  private readonly subCategoryGetAllUrl: string = '/api/subcategories/get_all'
  private readonly subCategoryGetByIdUrl: string = '/api/subcategories/get_by_id'

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
   * Inserta una sub categoria
   * @param subCategory
   * @returns Observable<ApiResponse>
   */
  create(subCategory: SubCategoryInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.subCategoryCreateUrl}`, subCategory, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Inserta varias sub categorias
   * @param subCategories
   * @returns Observable<ApiResponse>
   */
  createMany(subCategories: SubCategoryInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.subCategoryCreateManyUrl}`, subCategories, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza una sub categoria
   * @param subCategory
   * @returns Observable<ApiResponse>
   */
  update(subCategory: SubCategoryUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.subCategoryUpdateUrl}`, subCategory, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza varias sub categorias
   * @param subCategories
   * @returns Observable<ApiResponse>
   */
  updateMany(subCategories: SubCategoryUpdateDto[]): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.subCategoryUpdateManyUrl}`, subCategories, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina una sub categoria
   * @param subCategoryId
   * @returns Observable<ApiResponse>
   */
  delete(subCategoryId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.subCategoryDeleteUrl}/${subCategoryId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todas las sub categorias
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.subCategoryGetAllUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve una sub categoria
   * @param subCategoryId
   * @returns Observable<ApiResponse>
   */
  getById(subCategoryId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.subCategoryGetByIdUrl}/${subCategoryId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
