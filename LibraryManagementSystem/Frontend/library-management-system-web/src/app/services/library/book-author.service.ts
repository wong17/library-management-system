import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable } from 'rxjs';
import { BookAuthorInsertDto } from '../../entities/dtos/library/book-author-insert-dto';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class BookAuthorService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* BookAuthor urls */
  private readonly bookAuthorCreateUrl: string = '/api/book_authors/create'
  private readonly bookAuthorCreateManyUrl: string = '/api/book_authors/create_many'
  private readonly bookAuthorUpdateManyUrl: string = '/api/book_authors/update_many'
  private readonly bookAuthorDeleteUrl: string = '/api/book_authors/delete'
  private readonly bookAuthorGetAllUrl: string = '/api/book_authors/get_all'
  private readonly bookAuthorGetByIdUrl: string = '/api/book_authors/get_by_id'

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
   * Inserta un autor a un libro
   * @param bookAuthor
   * @returns Observable<ApiResponse>
   */
  create(bookAuthor: BookAuthorInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.bookAuthorCreateUrl}`, bookAuthor, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Inserta varios autores a un libro
   * @param bookAuthors
   * @returns Observable<ApiResponse>
   */
  createMany(bookAuthors: BookAuthorInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.bookAuthorCreateManyUrl}`, bookAuthors, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza varios autores de un libro
   * @param bookAuthors
   * @returns Observable<ApiResponse>
   */
  updateMany(bookAuthors: BookAuthorInsertDto[]): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.bookAuthorUpdateManyUrl}`, bookAuthors, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina un autor del libro
   * @param (bookId authorId)
   * @returns Observable<ApiResponse>
   */
  delete(bookId: number, authorId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.bookAuthorDeleteUrl}/${bookId}/${authorId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todos los autores de todos los libros
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookAuthorGetAllUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un autor del libro
   * @param (bookId authorId)
   * @returns Observable<ApiResponse>
   */
  getById(bookId: number, authorId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookAuthorGetByIdUrl}/${bookId}/${authorId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
