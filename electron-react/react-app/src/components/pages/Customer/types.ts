export interface ICustomerItem {
    id: number,
    firstName: string,
    secondName: string,
    email: string,
    phone: string,
    address?: string,
}

export interface ICustomerCreateItem {
    firstName: string,
    secondName: string,
    email: string,
    phone: string,
    address: string,
}

export interface ICustomerEditItem {
    id: number,
    firstName: string,
    secondName: string,
    email: string,
    phone: string,
    address: string,
}

export interface ICustomerServiceResponse {
    success: boolean,
    message: string,
    errors: string,
    payload: ICustomerItem[]
}

export interface ICustomerByIdServiceResponse {
    success: boolean,
    message: string,
    errors: string,
    payload: ICustomerItem
}

export interface ICustomerSearch {
    id: number | string | undefined;
    firstName: string;
    secondName: string;
    email: string;
    phone: string;
    address: string;
    projectId: number | string | undefined;
    page: number | string | undefined;
}

export interface ICustomerSearchResult {
    customers: Array<ICustomerItem>;
    pages: number;
    currentPage: number;
    total: number;
    projectName: string;
    projectId: number
}