import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthorInsertDto } from '../../entities/dtos/library/author-insert-dto';
import { ApiResponse } from '../../entities/api/api-response';
import { Observable } from 'rxjs';
import { AuthorUpdateDto } from '../../entities/dtos/library/author-update-dto';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Author urls */
  private readonly authorCreateUrl: string = '/api/Author/Create'
  private readonly authorCreateManyUrl: string = '/api/Author/CreateMany'
  private readonly authorUpdateUrl: string = '/api/Author/Update'
  private readonly authorUpdateManyUrl: string = '/api/Author/UpdateMany'
  private readonly authorUrl: string = '/api/Author'

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
   * Inserta un autor
   * @param author
   * @returns Observable<ApiResponse>
   */
  create(author: AuthorInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.authorCreateUrl}`, author, this.httpHeader);
  }

  /**
   * Inserta varios autores
   * @param authors
   * @returns Observable<ApiResponse>
   */
  createMany(authors: AuthorInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.authorCreateManyUrl}`, authors, this.httpHeader);
  }

  /**
   * Actualiza un autor
   * @param author
   * @returns Observable<ApiResponse>
   */
  update(author: AuthorUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.authorUpdateUrl}`, author, this.httpHeader);
  }

  /**
   * Actualiza varios autores
   * @param authors
   * @returns Observable<ApiResponse>
   */
  updateMany(authors: AuthorUpdateDto[]): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.authorUpdateManyUrl}`, authors, this.httpHeader);
  }

  /**
   * Elimina un autor
   * @param authorId
   * @returns Observable<ApiResponse>
   */
  delete(authorId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.authorUrl}/${authorId}`);
  }

  /**
   * Devuelve todos los autores
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.authorUrl}`);
  }

  /**
   * Devuelve un autor
   * @param authorId
   * @returns Observable<ApiResponse>
   */
  getById(authorId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.authorUrl}/${authorId}`);
  }
}
