import { CustomerModel } from "./customers.model";
import { JewelryShopModel } from "./jewelry-shop.model";
import { User } from "./user.model";
import { WorkMaterialModel } from "./work-materials.model";

export interface SubmitOrder{
    sketchBlob?: Blob;
    model: Order;
}

export interface PayOrderQuery {
    orderId: number;
    payment: number;
}

export interface Order{
    id: number;
    customer: CustomerModel;
    description: string;
    imageFileName: string;
    createdOn: Date;
    shop: JewelryShopModel;
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
    customerId?: number;
    status?: number
}

export interface OrderCraftingComponent {
    id: number;
    isDeposit: boolean;
    workMaterial: WorkMaterialModel;
    workMaterialPrice: number;
    quantity: number;
    totalComponentPrice: number
}

export enum OrderStatus {
    Opened = 1,
    Closed = 2
}
