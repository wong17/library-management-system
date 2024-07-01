import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable } from 'rxjs';
import { BookInsertDto } from '../../entities/dtos/library/book-insert-dto';
import { BookUpdateDto } from '../../entities/dtos/library/book-update-dto';
import { HttpErrorHandler } from '../../util/http-error-handler';
import { FilterBookDto } from '../../entities/dtos/library/filter-book-dto';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Book urls */
  private readonly bookCreateUrl: string = '/api/books/create'
  private readonly bookUpdateUrl: string = '/api/books/update'
  private readonly bookDeleteUrl: string = '/api/books/delete'
  private readonly bookGetAllUrl: string = '/api/books/get_all'
  private readonly bookGetByIdUrl: string = '/api/books/get_by_id'
  private readonly bookGetFilteredBookUrl: string = '/api/books/get_filtered_book'

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
   * Inserta un nuevo libro
   * @param book
   * @returns Observable<ApiResponse>
   */
  create(book: BookInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.bookCreateUrl}`, book, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza un libro
   * @param book
   * @returns Observable<ApiResponse>
   */
  update(book: BookUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.bookUpdateUrl}`, book, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina un libro
   * @param bookId
   * @returns Observable<ApiResponse>
   */
  delete(bookId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.bookDeleteUrl}/${bookId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todos los libros
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookGetAllUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un libro
   * @param bookId
   * @returns Observable<ApiResponse>
   */
  getById(bookId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookGetByIdUrl}/${bookId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelva una lista de libros filtrados por año, categoría, sub categoría, editorial y autores
   * @param filterBookDto 
   * @returns 
   */
  getFilteredBook(filterBookDto: FilterBookDto): Observable<ApiResponse> {
    const filterParamsDto = JSON.stringify(filterBookDto);

    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookGetFilteredBookUrl}`, {
      params: new HttpParams().set('filterParamsDto', filterParamsDto) })
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
