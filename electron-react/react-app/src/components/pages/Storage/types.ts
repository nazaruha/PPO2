import { IProductItem } from "../Product/types";
import { IProjectItem } from "../Project/types";

export interface IStorageItem {
    productId: number;
    product: IProductItem;
    projectId: number;
    project: IProjectItem;
    price: number;
    description: string;
    count: number;
    expireDate: Date | string;
}

export interface IStorageCreateItem {
    productId: number;
    projectId: number;
    price: number;
    description: string;
    count: number;
    manufacturerId: number;
    expireDate: Date | string;
}

export interface IStorageEditItem {
    productId: number;
    projectId: number;
    price: number;
    description: string;
    count: number;
    expireDate: Date | string;
}

export interface IStorageSearch {
    productName: string;
    manufacturerName: string;
    description: string;
    price: string | number;
    count: string | number;
    page: string | number;
}

export interface IStorageSearchResult {
    storage: IStorageItem[];
    pages: number;
    currentPage: number;
    total: number;
}