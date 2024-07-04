import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { environment } from "../../../environments/environment";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class CategorySignalRService {

    private categoryHubConnection: HubConnection;
    public categoryNotification = new Subject<boolean>();

    constructor() {
        this.categoryHubConnection = new HubConnectionBuilder()
            .withUrl(`${environment.apiUrl}/hubs/category_hub`)
            .build();

        this.startConnections();
    }

    private startConnections() {
        this.categoryHubConnection
            .start()
            .then(() => console.log('Connected to Monograph Hub'))
            .catch(err => console.error('Error connecting to BookLoan Hub:', err));

        this.categoryHubConnection.on('SendCategoryNotification', (value: boolean) => {
            this.categoryNotification.next(value);
        });
    }
}
