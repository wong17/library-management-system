import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { environment } from "../../../environments/environment";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class PublisherSignalRService {

    private publisherHubConnection: HubConnection;
    public publisherNotification = new Subject<boolean>();

    constructor() {
        this.publisherHubConnection = new HubConnectionBuilder()
            .withUrl(`${environment.apiUrl}/hubs/publisher_hub`)
            .build();

        this.startConnections();
    }

    private startConnections() {
        this.publisherHubConnection
            .start()
            .then(() => console.log('Connected to Monograph Hub'))
            .catch(err => console.error('Error connecting to BookLoan Hub:', err));

        this.publisherHubConnection.on('SendPublisherNotification', (value: boolean) => {
            this.publisherNotification.next(value);
        });
    }
}
