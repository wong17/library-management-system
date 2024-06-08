export interface DialogData {
    title: string;
    operation: DialogOperation;
    data?: object
}

export enum DialogOperation {
    Add = 'add',
    Update = 'update'
}