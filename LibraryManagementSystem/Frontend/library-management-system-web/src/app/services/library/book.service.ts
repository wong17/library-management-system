import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiResponse } from '../../entities/api/api-response';
import { Observable } from 'rxjs';
import { BookInsertDto } from '../../entities/dtos/library/book-insert-dto';
import { BookUpdateDto } from '../../entities/dtos/library/book-update-dto';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Book urls */
  private readonly bookCreateUrl: string = '/api/Book/Create'
  private readonly bookUpdateUrl: string = '/api/Book/Update'
  private readonly bookUrl: string = '/api/Book'

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
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.bookCreateUrl}`, book, this.httpHeader);
  }

  /**
   * Actualiza un libro
   * @param book
   * @returns Observable<ApiResponse>
   */
  update(book: BookUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.bookUpdateUrl}`, book, this.httpHeader);
  }

  /**
   * Elimina un libro
   * @param bookId
   * @returns Observable<ApiResponse>
   */
  delete(bookId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.bookUrl}/${bookId}`);
  }

  /**
   * Devuelve todos los libros
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookUrl}`);
  }

  /**
   * Devuelve un libro
   * @param bookId
   * @returns Observable<ApiResponse>
   */
  getById(bookId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookUrl}/${bookId}`);
  }
}
