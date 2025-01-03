import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RoleInsertDto } from '../../entities/dtos/security/role-insert-dto';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable } from 'rxjs';
import { RoleUpdateDto } from '../../entities/dtos/security/role-update-dto';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Role urls */
  private readonly roleCreateUrl: string = '/api/roles/create'
  private readonly roleUpdateUrl: string = '/api/roles/update'
  private readonly roleDeleteUrl: string = '/api/roles/delete'
  private readonly roleGetAllUrl: string = '/api/roles/get_all'
  private readonly roleGetByIdUrl: string = '/api/roles/get_by_id'

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
   * Inserta un nuevo rol
   * @param role
   * @returns Observable<ApiResponse>
   */
  create(role: RoleInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.roleCreateUrl}`, role, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza un rol
   * @param role
   * @returns Observable<ApiResponse>
   */
  update(role: RoleUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.roleUpdateUrl}`, role, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina un rol
   * @param roleId
   * @returns Observable<ApiResponse>
   */
  delete(roleId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.roleDeleteUrl}/${roleId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todos los roles
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.roleGetAllUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un rol
   * @param roleId
   * @returns Observable<ApiResponse>
   */
  getById(roleId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.roleGetByIdUrl}/${roleId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

}
