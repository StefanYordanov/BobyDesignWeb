import { CustomerModel } from "./customers.model";
import { User } from "./user.model";
import { WorkMaterialModel } from "./work-materials.model";

export interface Order{
    id: number;
    customer: CustomerModel;
    description: string;
    createdOn: Date;
    finishingDate: Date;
    craftingComponents: OrderCraftingComponent[];
    laborPrice: number;
    totalPrice: number;
    deposit: number;
    status: OrderStatus;
    shopUser: User;
}

export interface OrderQuery {
    fromDate?: string;
    toDate?: string;
    searchPhrase?: string;
    customerId?: number
}

export interface OrderCraftingComponent {
    workMaterial: WorkMaterialModel;
    workMaterialPrice: number;
    quantity: number;
}

export enum OrderStatus {
    Opened = 0,
    Closed = 1
}