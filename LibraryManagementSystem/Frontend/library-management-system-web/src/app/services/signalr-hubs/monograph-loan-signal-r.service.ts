import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MonographLoanSignalRService {

  private monographLoanHubConnection: HubConnection;
  public monographLoanNotification = new Subject<boolean>();
  
  constructor() { 
    this.monographLoanHubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/hubs/monographloan_hub`)
      .build();

      this.startConnections();
  }

  private startConnections() {
    this.monographLoanHubConnection
      .start()
      .then(() => console.log('Connected to MonographLoan Hub'))
      .catch(err => console.error('Error connecting to MonographLoan Hub:', err));
      
    this.monographLoanHubConnection.on('SendLoanNotification', (loanCreated: boolean) => {
      this.monographLoanNotification.next(loanCreated);
    });
  }
}
