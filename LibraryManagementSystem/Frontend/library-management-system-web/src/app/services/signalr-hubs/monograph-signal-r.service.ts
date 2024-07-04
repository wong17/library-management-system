import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class MonographSignalRService {

    private monographHubConnection: HubConnection;
    public monographNotification = new Subject<boolean>();

    constructor() {
        this.monographHubConnection = new HubConnectionBuilder()
            .withUrl(`${environment.apiUrl}/hubs/monograph_hub`)
            .build();

        this.startConnections();
    }

    private startConnections() {
        this.monographHubConnection
            .start()
            .then(() => console.log('Connected to Monograph Hub'))
            .catch(err => console.error('Error connecting to BookLoan Hub:', err));

        this.monographHubConnection.on('SendMonographNotification', (value: boolean) => {
            this.monographNotification.next(value);
        });
    }
}
