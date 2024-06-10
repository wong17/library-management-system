import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BookSubCategoryInsertDto } from '../../entities/dtos/library/book-sub-category-insert-dto';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable } from 'rxjs';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class BookSubCategoryService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* BookSubCategory urls */
  private readonly bookSubCategoryCreateUrl: string = '/api/BookSubCategory/Create'
  private readonly bookSubCategoryCreateManyUrl: string = '/api/BookSubCategory/CreateMany'
  private readonly bookSubCategoryUpdateManyUrl: string = '/api/BookSubCategory/UpdateMany'
  private readonly bookSubCategoryUrl: string = '/api/BookSubCategory'

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
   * Inserta una sub categoria a un libro
   * @param bookSubCategory
   * @returns Observable<ApiResponse>
   */
  create(bookSubCategory: BookSubCategoryInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.bookSubCategoryCreateUrl}`, bookSubCategory, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Inserta varias sub categorias a un libro
   * @param bookSubCategories
   * @returns Observable<ApiResponse>
   */
  createMany(bookSubCategories: BookSubCategoryInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.bookSubCategoryCreateManyUrl}`, bookSubCategories, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza varias sub categorias a un libro
   * @param bookSubCategories
   * @returns Observable<ApiResponse>
   */
  updateMany(bookSubCategories: BookSubCategoryInsertDto[]): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.bookSubCategoryUpdateManyUrl}`, bookSubCategories, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina una sub categoria del libro
   * @param (bookId subCategoryId)
   * @returns Observable<ApiResponse>
   */
  delete(bookId: number, subCategoryId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.bookSubCategoryUrl}/${bookId}/${subCategoryId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todas las sub categorias de todos los libros
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookSubCategoryUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve una sub categoria de un libro
   * @param (bookId subCategoryId)
   * @returns Observable<ApiResponse>
   */
  getById(bookId: number, subCategoryId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookSubCategoryUrl}/${bookId}/${subCategoryId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
