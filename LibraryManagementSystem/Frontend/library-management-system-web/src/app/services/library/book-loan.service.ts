import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BookLoanInsertDto } from '../../entities/dtos/library/book-loan-insert-dto';
import { ApiResponse } from '../../entities/api/api-response';
import { catchError, Observable } from 'rxjs';
import { HttpErrorHandler } from '../../util/http-error-handler';

@Injectable({
  providedIn: 'root'
})
export class BookLoanService {

  /* API url base */
  private readonly apiUrl: string = environment.apiUrl;
  /* BookLoan urls */
  private readonly bookLoanCreateUrl: string = '/api/BookLoan/Create'
  private readonly bookLoanUpdateBorrowedBookLoanUrl: string = '/api/BookLoan/UpdateBorrowedBookLoan'
  private readonly bookLoanUpdateReturnedBookLoanUrl: string = '/api/BookLoan/UpdateReturnedBookLoan'
  private readonly bookLoanUrl: string = '/api/BookLoan'

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
   * Inserta un préstamo de libro
   * @param bookLoan
   * @returns Observable<ApiResponse>
   */
  create(bookLoan: BookLoanInsertDto): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(`${this.apiUrl}${this.bookLoanCreateUrl}`, bookLoan, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza la fecha de devolución de una solicitud préstamo de libro
   * @param (bookLoanId dueDate)
   * @returns Observable<ApiResponse>
   */
  updateBorrowedBookLoan(bookLoanId: number, dueDate: Date): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.bookLoanUpdateBorrowedBookLoanUrl}/${bookLoanId}/${dueDate}`, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Actualiza una solicitud de préstamo de libro cambiando su estado a DEVUELTO
   * @param bookLoanId
   * @returns Observable<ApiResponse>
   */
  updateReturnedBookLoan(bookLoanId: number): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.bookLoanUpdateReturnedBookLoanUrl}/${bookLoanId}`, this.httpHeader)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Elimina un préstamo de libro
   * @param bookLoanId
   * @returns Observable<ApiResponse>
   */
  delete(bookLoanId: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.bookLoanUrl}/${bookLoanId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todos los préstamos de libros
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookLoanUrl}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve un préstamo de libro
   * @param bookLoanId
   * @returns Observable<ApiResponse>
   */
  getById(bookLoanId: number): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookLoanUrl}/${bookLoanId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
