import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../entities/api/api-response';
import { Observable } from 'rxjs';
import { UserInsertDto } from '../../entities/dtos/security/user-insert-dto';
import { UserUpdateDto } from '../../entities/dtos/security/user-update-dto';
import { UserLogInDto } from '../../entities/dtos/security/user-log-in-dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* User urls */
  private readonly userCreateUrl: string = '/api/User/Create'
  private readonly userLogInUrl: string = '/api/User/LogIn';
  private readonly userUpdateUrl: string = '/api/User/Update'
  private readonly userUrl: string = '/api/User';

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
   * Inserta un nuevo usuario
   * @param user
   * @returns Observable<ApiResponse>
   */
  create(user: UserInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.userCreateUrl}`, user, this.httpHeader);
  }

  /**
   * Inserta un nuevo usuario
   * @param user
   * @returns Observable<ApiResponse>
   */
  login(user: UserLogInDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.userLogInUrl}`, user, this.httpHeader);
  }

  /**
   * Actualiza un usuario
   * @param user
   * @returns
   */
  update(user: UserUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.userUpdateUrl}`, user, this.httpHeader);
  }

  /**
   * Elimina un usuario
   * @param userId
   * @returns Observable<ApiResponse>
   */
  delete(userId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.userUrl}/${userId}`);
  }

  /**
   * Devuelve todos los usuarios
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.userUrl}`);
  }

  /**
   * Devuelve un usuario
   * @param id
   * @returns Observable<ApiResponse>
   */
  getById(userId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.userUrl}/${userId}`);
  }
  
}
