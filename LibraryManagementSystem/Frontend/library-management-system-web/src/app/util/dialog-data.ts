export interface DialogData {
    title: string;
    operation: DialogOperation;
    data?: object
}

export interface LoanDialogData {
    title: string;
    typeOfLoan: TypeOfLoan;
    data?: object
}

export enum DialogOperation {
    Add = 'add',
    Update = 'update'
}

export enum TypeOfLoan {
    Home = 'domicilio',
    Library = 'sala'
}