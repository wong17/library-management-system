import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable } from 'rxjs';
import { MonographLoanInsertDto } from '../../entities/dtos/library/monograph-loan-insert-dto';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class MonographLoanService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* MonographLoan urls */
  private readonly monographLoanCreateUrl: string = '/api/MonographLoan/Create'
  private readonly monographLoanUpdateBorrowedMonographLoanUrl: string = '/api/MonographLoan/UpdateBorrowedBookLoan'
  private readonly monographLoanUpdateReturnedMonographLoanUrl: string = '/api/MonographLoan/UpdateReturnedBookLoan'
  private readonly monographLoanUrl: string = '/api/MonographLoan'

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
   * Inserta un préstamo de monografía
   * @param monographLoan
   * @returns Observable<ApiResponse>
   */
  create(monographLoan: MonographLoanInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.monographLoanCreateUrl}`, monographLoan, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza la fecha de devolución de una solicitud préstamo de monografía
   * @param (monographLoanId dueDate)
   * @returns Observable<ApiResponse>
   */
  updateBorrowedMonographLoan(monographLoanId: number, dueDate: Date): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.monographLoanUpdateBorrowedMonographLoanUrl}/${monographLoanId}/${dueDate}`, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza una solicitud de préstamo de monografía cambiando su estado a DEVUELTA
   * @param monographLoanId
   * @returns Observable<ApiResponse>
   */
  updateReturnedMonographLoan(monographLoanId: number): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.monographLoanUpdateReturnedMonographLoanUrl}/${monographLoanId}`, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina un préstamo de monografía
   * @param monographLoanId
   * @returns Observable<ApiResponse>
   */
  delete(monographLoanId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.monographLoanUrl}/${monographLoanId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todos los préstamos de monografías
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographLoanUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un préstamo de monografía
   * @param monographLoanId
   * @returns Observable<ApiResponse>
   */
  getById(monographLoanId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographLoanUrl}/${monographLoanId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
