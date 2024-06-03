import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiResponse } from '../../entities/api/api-response';
import { Observable } from 'rxjs';
import { MonographAuthorInsertDto } from '../../entities/dtos/library/monograph-author-insert-dto';

@Injectable({
  providedIn: 'root'
})
export class MonographAuthorService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* MonographAuthor urls */
  private readonly monographAuthorCreateUrl: string = '/api/MonographAuthor/Create'
  private readonly monographAuthorCreateManyUrl: string = '/api/MonographAuthor/CreateMany'
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
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.monographAuthorCreateUrl}`, monographAuthor, this.httpHeader);
  }

  /**
   * Inserta varios autores a una monografía
   * @param monographAuthors
   * @returns Observable<ApiResponse>
   */
  createMany(monographAuthors: MonographAuthorInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.monographAuthorCreateManyUrl}`, monographAuthors, this.httpHeader);
  }

  /**
   * Elimina un autor de la monografía
   * @param (monographId authorId)
   * @returns Observable<ApiResponse>
   */
  delete(monographId: number, authorId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.monographAuthorUrl}/${monographId}/${authorId}`);
  }

  /**
   * Devuelve todos los autores de todas las monografías
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographAuthorUrl}`);
  }

  /**
   * Devuelve un autor de la monografía
   * @param (monographId authorId)
   * @returns Observable<ApiResponse>
   */
  getById(monographId: number, authorId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographAuthorUrl}/${monographId}/${authorId}`);
  }
}
