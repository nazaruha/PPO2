export interface IProductItem {
    id: number;
    name: string;
    manufacturerId: number;
    manufacturerName: string;
}

export interface IProductCreateItem {
    name: string;
    manufacturerId: number;
}

export interface IProductEditItem {
    id: number;
    name: string;
    manufacturerId: number;
}

export interface IProductSearch {
    name: string;
    manufacturerName: string;
    page: number | string;
}

export interface IProductSearchResult {
    products: IProductItem[];
    pages: number;
    currentPage: number;
    total: number;
}