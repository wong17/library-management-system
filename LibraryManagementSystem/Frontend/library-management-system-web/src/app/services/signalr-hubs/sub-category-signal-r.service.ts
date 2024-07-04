import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { environment } from "../../../environments/environment";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class SubCategorySignalRService {

    private subCategoryHubConnection: HubConnection;
    public subCategoryNotification = new Subject<boolean>();

    constructor() {
        this.subCategoryHubConnection = new HubConnectionBuilder()
            .withUrl(`${environment.apiUrl}/hubs/sub_category_hub`)
            .build();

        this.startConnections();
    }

    private startConnections() {
        this.subCategoryHubConnection
            .start()
            .then(() => console.log('Connected to Monograph Hub'))
            .catch(err => console.error('Error connecting to BookLoan Hub:', err));

        this.subCategoryHubConnection.on('SendSubCategoryNotification', (value: boolean) => {
            this.subCategoryNotification.next(value);
        });
    }
}
