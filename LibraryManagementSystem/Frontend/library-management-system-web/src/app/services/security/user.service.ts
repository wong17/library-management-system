import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, map, Observable } from 'rxjs';
import { UserInsertDto } from '../../entities/dtos/security/user-insert-dto';
import { UserUpdateDto } from '../../entities/dtos/security/user-update-dto';
import { UserLogInDto } from '../../entities/dtos/security/user-log-in-dto';
import { HttpErrorHandler } from '../../util/http-error-handler';
import { UserDto } from '../../entities/dtos/security/user-dto';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* User urls */
  private readonly userCreateUrl: string = '/api/users/create'
  private readonly userLogInUrl: string = '/api/users/log_in'
  private readonly userUpdateUrl: string = '/api/users/update'
  private readonly userDeleteUrl: string = '/api/users/delete'
  private readonly userGetAllUrl: string = '/api/users/get_all'
  private readonly userGetByIdUrl: string = '/api/users/get_by_id'

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
  constructor(private readonly http: HttpClient, @Inject(PLATFORM_ID) private platformId: Object) { }

  /**
   * Inserta un nuevo usuario
   * @param user
   * @returns Observable<ApiResponse>
   */
  create(user: UserInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.userCreateUrl}`, user, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Para iniciar sesión
   * @param user
   * @returns Observable<ApiResponse>
   */
  login(user: UserLogInDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.userLogInUrl}`, user, this.httpHeader)
      .pipe(
        map(response => {
          if (response.isSuccess === 0 && response.statusCode === 200) {
            const userDto = response.result as UserDto;
            this.setCurrentUser(userDto);
          }
          return response;
        }),
        catchError(error => HttpErrorHandler.handlerError(error))
      );
  }

  /* Guardar informacion del usuario que inicio sesion */
  private setCurrentUser(user: UserDto): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('currentUser', JSON.stringify(user));
    }
  }

  /* Para cerrar la sesión actual */
  logout() {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('currentUser');
    }
  }

  /* Para obtener la información del usuario que inició sesión */
  get currentUser(): UserDto | null {
    if (!isPlatformBrowser(this.platformId)) 
      return null;

    const userJson = localStorage.getItem('currentUser');
    return userJson ? JSON.parse(userJson) : null;
  }

  /**
   * Actualiza un usuario
   * @param user
   * @returns Observable<ApiResponse>
   */
  update(user: UserUpdateDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.userUpdateUrl}`, user, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina un usuario
   * @param userId
   * @returns Observable<ApiResponse>
   */
  delete(userId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.userDeleteUrl}/${userId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todos los usuarios
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.userGetAllUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un usuario
   * @param userId
   * @returns Observable<ApiResponse>
   */
  getById(userId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.userGetByIdUrl}/${userId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

}
