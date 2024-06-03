import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CategoryInsertDto } from '../../entities/dtos/library/category-insert-dto';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../entities/api/api-response';
import { CategoryUpdateDto } from '../../entities/dtos/library/category-update-dto';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Category urls */
  private readonly categoryCreateUrl: string = '/api/Category/Create'
  private readonly categoryCreateManyUrl: string = '/api/Category/CreateMany'
  private readonly categoryUpdateUrl: string = '/api/Category/Update'
  private readonly categoryUpdateManyUrl: string = '/api/Category/UpdateMany'
  private readonly categoryUrl: string = '/api/Category'

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
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.categoryCreateUrl}`, category, this.httpHeader);
  }

  /**
   * Inserta varias categorias
   * @param categories
   * @returns Observable<ApiResponse>
   */
  createMany(categories: CategoryInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.categoryCreateManyUrl}`, categories, this.httpHeader);
  }

  /**
   * Actualiza un categoria
   * @param category
   * @returns Observable<ApiResponse>
   */
  update(category: CategoryUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.categoryUpdateUrl}`, category, this.httpHeader);
  }

  /**
   * Actualiza varias categorias
   * @param categories
   * @returns Observable<ApiResponse>
   */
  updateMany(categories: CategoryUpdateDto[]): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.categoryUpdateManyUrl}`, categories, this.httpHeader);
  }

  /**
   * Elimina una categoria
   * @param categoryId
   * @returns Observable<ApiResponse>
   */
  delete(categoryId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.categoryUrl}/${categoryId}`);
  }

  /**
   * Devuelve todas las categorias
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.categoryUrl}`);
  }

  /**
   * Devuelve una categoria
   * @param categoryId
   * @returns Observable<ApiResponse>
   */
  getById(categoryId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.categoryUrl}/${categoryId}`);
  }
}
