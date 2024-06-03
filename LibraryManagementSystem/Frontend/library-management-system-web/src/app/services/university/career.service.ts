import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../entities/api/api-response';

@Injectable({
  providedIn: 'root'
})
export class CareerService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* Career urls */
  private readonly careerUrl: string = '/api/Career'

  /**
   * Modulo HttpClient para enviar solicitudes http
   * @param http
   */
  constructor(private readonly http: HttpClient) { }

  /**
   * Devuelve todas las carreras
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.careerUrl}`);
  }

  /**
   * Devuelve una carrera
   * @param careerId
   * @returns Observable<ApiResponse>
   */
  getById(careerId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.careerUrl}/${careerId}`);
  }
}