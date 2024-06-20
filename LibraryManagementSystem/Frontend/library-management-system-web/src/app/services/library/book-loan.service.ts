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
  private readonly bookLoanCreateUrl: string = '/api/book_loans/create'
  private readonly bookLoanUpdateBorrowedBookLoanUrl: string = '/api/book_loans/update_borrowed_book_loan'
  private readonly bookLoanUpdateReturnedBookLoanUrl: string = '/api/book_loans/update_returned_book_loan'
  private readonly bookLoanDeleteUrl: string = '/api/book_loans/delete'
  private readonly bookLoanGetAllUrl: string = '/api/book_loans/get_all'
  private readonly bookLoanGetByIdUrl: string = '/api/book_loans/get_by_id'

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
    return this.http.put<ApiResponse>(`${this.apiUrl}${this.bookLoanUpdateBorrowedBookLoanUrl}/${bookLoanId}/${dueDate.toDateString()}`, this.httpHeader)
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
    return this.http.delete<ApiResponse>(`${this.apiUrl}${this.bookLoanDeleteUrl}/${bookLoanId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }

  /**
   * Devuelve todos los préstamos de libros
   * @returns Observable<ApiResponse>
   */
  getAll(): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookLoanGetAllUrl}`)
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
    return this.http.get<ApiResponse>(`${this.apiUrl}${this.bookLoanGetByIdUrl}/${bookLoanId}`)
      .pipe<ApiResponse>(catchError(error => {
        return HttpErrorHandler.handlerError(error);
      }));
  }
}
