import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { environment } from "../../../environments/environment";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class AuthorSignalRService {

    private authorHubConnection: HubConnection;
    public authorNotification = new Subject<boolean>();

    constructor() {
        this.authorHubConnection = new HubConnectionBuilder()
            .withUrl(`${environment.apiUrl}/hubs/author_hub`)
            .build();

        this.startConnections();
    }

    private startConnections() {
        this.authorHubConnection
            .start()
            .then(() => console.log('Connected to Monograph Hub'))
            .catch(err => console.error('Error connecting to BookLoan Hub:', err));

        this.authorHubConnection.on('SendAuthorNotification', (value: boolean) => {
            this.authorNotification.next(value);
        });
    }
}
