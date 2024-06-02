import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RoleInsertDto } from '../../entities/dtos/security/role-insert-dto';
import { ApiResponse } from '../../entities/api/api-response';
import { Observable } from 'rxjs';
import { RoleUpdateDto } from '../../entities/dtos/security/role-update-dto';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Role urls */
  private readonly roleCreateUrl: string = '/api/Role/Create'
  private readonly roleUpdateUrl: string = '/api/Role/Update'
  private readonly roleUrl: string = '/api/Role';

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
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.roleCreateUrl}`, role, this.httpHeader);
  }

  /**
   * Actualiza un rol
   * @param role
   * @returns Observable<ApiResponse>
   */
  update(role: RoleUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.roleUpdateUrl}`, role, this.httpHeader);
  }

  /**
   * Elimina un rol
   * @param roleId
   * @returns Observable<ApiResponse>
   */
  delete(roleId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.roleUrl}/${roleId}`);
  }

  /**
   * Devuelve todos los roles
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.roleUrl}`);
  }

  /**
   * Devuelve un rol
   * @param roleId
   * @returns Observable<ApiResponse>
   */
  getById(roleId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.roleUrl}/${roleId}`);
  }
  
}
