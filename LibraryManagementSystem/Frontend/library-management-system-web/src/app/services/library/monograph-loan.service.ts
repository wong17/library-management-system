import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable, of } from 'rxjs';
import { MonographLoanInsertDto } from '../../entities/dtos/library/monograph-loan-insert-dto';
import { HttpErrorHandler } from '../../util/http-error-handler';
import { UpdateBorrowedMonographLoanDto } from '../../entities/dtos/library/update-borrowed-monograph-loan-dto';
import { UpdateReturnedMonographLoanDto } from '../../entities/dtos/library/update-returned-monograph-loan-dto';

@Injectable({
  providedIn: 'root'
})
export class MonographLoanService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* MonographLoan urls */
  private readonly monographLoanCreateUrl: string = '/api/monograph_loans/create'
  private readonly monographLoanUpdateBorrowedMonographLoanUrl: string = '/api/monograph_loans/update_borrowed_monograph_loan'
  private readonly monographLoanUpdateReturnedMonographLoanUrl: string = '/api/monograph_loans/update_returned_monograph_loan'
  private readonly monographLoanDeleteUrl: string = '/api/monograph_loans/delete'
  private readonly monographLoanGetAllUrl: string = '/api/monograph_loans/get_all'
  private readonly monographLoanGetByIdUrl: string = '/api/monograph_loans/get_by_id'
  private readonly monographLoanGetByStateUrl: string = '/api/monograph_loans/get_by_state'
  private readonly monographLoanGetByStudentCarnetUrl: string = '/api/monograph_loans/get_by_student_carnet'

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
  updateBorrowedMonographLoan(loanDto: UpdateBorrowedMonographLoanDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.monographLoanUpdateBorrowedMonographLoanUrl}`, loanDto, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza una solicitud de préstamo de monografía cambiando su estado a DEVUELTA
   * @param monographLoanId
   * @returns Observable<ApiResponse>
   */
  updateReturnedMonographLoan(loanDto: UpdateReturnedMonographLoanDto): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.monographLoanUpdateReturnedMonographLoanUrl}`, loanDto, this.httpHeader)
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
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.monographLoanDeleteUrl}/${monographLoanId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todos los préstamos de monografías
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographLoanGetAllUrl}`)
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
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographLoanGetByIdUrl}/${monographLoanId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un préstamo de monografía
   * @param state
   * @returns Observable<ApiResponse>
   */
  getByState(state: string): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographLoanGetByStateUrl}/${state}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un préstamo de monografía
   * @param carnet
   * @returns Observable<ApiResponse>
   */
  getByStudentCarnet(carnet: string): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.monographLoanGetByStudentCarnetUrl}/${carnet}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
