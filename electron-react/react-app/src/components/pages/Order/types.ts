import { ICustomerItem } from "../Customer/types";
import { IProductItem } from "../Product/types";
import { IProjectItem } from "../Project/types";

export interface IOrderItem {
    id: number;
    customerId: number;
    customer: ICustomerItem;
    projectId: number;
    project: IProjectItem;
    productId: number;
    product: IProductItem;
    totalPrice: number;
    productQuantity: number;
    sellDate: Date | string;
}

export interface IOrderCreateItem {
    customerId: number;
    projectId: number;
    productId: number;
    totalPrice: number;
    productQuantity: number;
    sellDate: Date | string;
    manufacturerId: number; // helper prop
}

export interface IOrderEditItem {
    id: number;
    customerId: number;
    customer: ICustomerItem | undefined;
    projectId: number;
    // project: IProjectItem;
    productId: number;
    product: IProductItem | undefined;
    totalPrice: number;
    productQuantity: number;
    sellDate: Date | string;
    manufacturerId: number;
}

export interface IOrderSearch {
    id: number | string;
    projectId: number | string;
    customerFirstName: string;
    customerSecondName: string;
    productName: string;
    manufacturerName: string;
    totalPrice: number | string;
    productQuantity: number | string;
    page: number | string;
}

export interface IOrderSearchResult {
    orders: IOrderItem[];
    pages: number;
    currentPage: number;
    total: number;
}