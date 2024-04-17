import { OrderCraftingComponent } from "./order.model";

export interface OrderItemReportItem extends OrderCraftingComponent{
    order: OrderReportItem;
}

export interface OrderReportItem {
    id: number;
    orderCreatedOn: Date;
}

export interface OrderItemsReport {
    items: OrderItemReportItem[];
    totalSoldQuantity: number;
    totalSoldRevenue: number;
    totalDepositQuantity: number;
    totalDepositRevenue: number;
    totalQuantity: number;
    totalRevenue: number;
}

export interface WorkMaterialReportQuery {
    workMaterialId: number;
    fromDate?: string;
    toDate?: string;
    orderStatus?: number;
    orderType?: number;
    orderPaymentMethod?: number;
}