export interface WorkMaterialModel {
    id: number;
    name: string;
    measuringUnit: string;
    pricingType: MaterialPricingType
    relevantPrice?: LatestWorkMaterialRelevantPriceModel,
    quantity: number;
    reservedQuantity: number;
}

export enum MaterialPricingType {
    Fixed = 0,
    PerGram = 1,
}

export interface LatestWorkMaterialRelevantPriceModel {
    id: number;
    workMaterialId: number;
    sellingPrice: number;
    purchasingPrice: number;
    lastUpdatedOn: Date;
}