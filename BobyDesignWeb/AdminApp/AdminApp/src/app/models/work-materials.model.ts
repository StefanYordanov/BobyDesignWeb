export interface WorkMaterialModel {
    id: number;
    name: string;
    pricingType: MaterialPricingType
    relevantPrice?: LatestWorkMaterialRelevantPriceModel
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