import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../entities/api/api-response';
import { Observable } from 'rxjs';
import { PublisherInsertDto } from '../../entities/dtos/library/publisher-insert-dto';
import { PublisherUpdateDto } from '../../entities/dtos/library/publisher-update-dto';

@Injectable({
  providedIn: 'root'
})
export class PublisherService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Publisher urls */
  private readonly publisherCreateUrl: string = '/api/Publisher/Create'
  private readonly publisherCreateManyUrl: string = '/api/Publisher/CreateMany'
  private readonly publisherUpdateUrl: string = '/api/Publisher/Update'
  private readonly publisherUpdateManyUrl: string = '/api/Publisher/UpdateMany'
  private readonly publisherUrl: string = '/api/Publisher'

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
   * Inserta una editorial
   * @param publisher
   * @returns Observable<ApiResponse>
   */
  create(publisher: PublisherInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.publisherCreateUrl}`, publisher, this.httpHeader);
  }

  /**
   * Inserta varias editoriales
   * @param publishers
   * @returns Observable<ApiResponse>
   */
  createMany(publishers: PublisherInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.publisherCreateManyUrl}`, publishers, this.httpHeader);
  }

  /**
   * Actualiza una editorial
   * @param publisher
   * @returns Observable<ApiResponse>
   */
  update(publisher: PublisherUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.publisherUpdateUrl}`, publisher, this.httpHeader);
  }

  /**
   * Actualiza varias editoriales
   * @param publishers
   * @returns Observable<ApiResponse>
   */
  updateMany(publishers: PublisherUpdateDto[]): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.publisherUpdateManyUrl}`, publishers, this.httpHeader);
  }

  /**
   * Elimina una editorial
   * @param publisherId
   * @returns Observable<ApiResponse>
   */
  delete(publisherId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.publisherUrl}/${publisherId}`);
  }

  /**
   * Devuelve todas las editoriales
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.publisherUrl}`);
  }

  /**
   * Devuelve una editorial
   * @param publisherId
   * @returns Observable<ApiResponse>
   */
  getById(publisherId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.publisherUrl}/${publisherId}`);
  }
}
