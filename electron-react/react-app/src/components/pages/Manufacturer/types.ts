export interface IManufacturerItem {
    id: number;
    name: string;
}

export interface IManufacturerCreateItem {
    name: string;
}

export interface IManufacturerEditItem {
    id: number;
    name: string;
}

export interface IManufacturerSearch {
    name: string;
    page: string | number;
}

export interface IManufacturerSearchResult {
    manufacturers: IManufacturerItem[],
    pages: number;
    currentPage: number;
    total: number;
}

export interface IManufacturerServiceResponse {
    success: boolean;
    message: string;
    payload: IManufacturerItem | IManufacturerItem[];
    errors: string
}