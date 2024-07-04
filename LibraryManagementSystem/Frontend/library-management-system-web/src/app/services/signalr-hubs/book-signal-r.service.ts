import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class BookSignalRService {

    private bookHubConnection: HubConnection;
    public bookNotification = new Subject<boolean>();

    constructor() {
        this.bookHubConnection = new HubConnectionBuilder()
            .withUrl(`${environment.apiUrl}/hubs/book_hub`)
            .build();

        this.startConnections();
    }

    private startConnections() {
        this.bookHubConnection
            .start()
            .then(() => console.log('Connected to Book Hub'))
            .catch(err => console.error('Error connecting to BookLoan Hub:', err));

        this.bookHubConnection.on('SendBookNotification', (value: boolean) => {
            this.bookNotification.next(value);
        });
    }
}
