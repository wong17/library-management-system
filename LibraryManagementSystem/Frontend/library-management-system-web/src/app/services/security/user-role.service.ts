import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserRoleInsertDto } from '../../entities/dtos/security/user-role-insert-dto';
import { catchError, Observable } from 'rxjs';
import { ApiResponse } from '../../entities/api/api-response';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class UserRoleService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* UserRole urls */
  private readonly userRoleCreateUrl: string = '/api/user_roles/create'
  private readonly userRoleCreateManyUrl: string = '/api/user_roles/create_many'
  private readonly userRoleDeleteUrl: string = '/api/user_roles/delete'
  private readonly userRoleGetAllUrl: string = '/api/user_roles/get_all'
  private readonly userRoleGetByIdUrl: string = '/api/user_roles/get_by_id'

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
   * Inserta un rol a un usuario
   * @param userRole
   * @returns Observable<ApiResponse>
   */
  create(userRole: UserRoleInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.userRoleCreateUrl}`, userRole, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Inserta varios roles a un usuario
   * @param userRoles
   * @returns Observable<ApiResponse>
   */
  createMany(userRoles: UserRoleInsertDto[]): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.userRoleCreateManyUrl}`, userRoles, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina un rol del usuario
   * @param (userId roleId)
   * @returns Observable<ApiResponse>
   */
  delete(userId: number, roleId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.userRoleDeleteUrl}/${userId}/${roleId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todos los roles de todos los usuarios
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.userRoleGetAllUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un rol
   * @param (userId roleId)
   * @returns Observable<ApiResponse>
   */
  getById(userId: number, roleId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.userRoleGetByIdUrl}/${userId}/${roleId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
