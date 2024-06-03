import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiResponse } from '../../entities/api/api-response';
import { Observable } from 'rxjs';
import { BookAuthorInsertDto } from '../../entities/dtos/library/book-author-insert-dto';

@Injectable({
  providedIn: 'root'
})
export class BookAuthorService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* BookAuthor urls */
  private readonly bookAuthorCreateUrl: string = '/api/BookAuthor/Create'
  private readonly bookAuthorCreateManyUrl: string = '/api/BookAuthor/CreateMany'
  private readonly bookAuthorUrl: string = '/api/BookAuthor'

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
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.bookAuthorCreateUrl}`, bookAuthor, this.httpHeader);
  }

  /**
   * Inserta varios autores a un libro
   * @param bookAuthors
   * @returns Observable<ApiResponse>
   */
  createMany(bookAuthors: BookAuthorInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.bookAuthorCreateManyUrl}`, bookAuthors, this.httpHeader);
  }

  /**
   * Elimina un autor del libro
   * @param (bookId authorId)
   * @returns Observable<ApiResponse>
   */
  delete(bookId: number, authorId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.bookAuthorUrl}/${bookId}/${authorId}`);
  }

  /**
   * Devuelve todos los autores de todos los libros
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookAuthorUrl}`);
  }

  /**
   * Devuelve un autor del libro
   * @param (bookId authorId)
   * @returns Observable<ApiResponse>
   */
  getById(bookId: number, authorId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookAuthorUrl}/${bookId}/${authorId}`);
  }
}
