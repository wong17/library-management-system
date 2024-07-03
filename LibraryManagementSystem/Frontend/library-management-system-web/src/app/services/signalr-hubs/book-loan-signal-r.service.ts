import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BookLoanSignalRService {

  private bookLoanHubConnection: HubConnection;
  public bookLoanNotification = new Subject<boolean>();

  constructor() {
    this.bookLoanHubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/bookloan_hub`)
      .build();

    this.startConnections();
  }

  private startConnections() {
    this.bookLoanHubConnection
      .start()
      .then(() => console.log('Connected to BookLoan Hub'))
      .catch(err => console.error('Error connecting to BookLoan Hub:', err));

    this.bookLoanHubConnection.on('SendLoanNotification', (loanCreated: boolean) => {
      this.bookLoanNotification.next(loanCreated);
    });
  }
}
