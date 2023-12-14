import { Injectable } from "@angular/core";
import { OrderCraftingComponent, Order } from "../models/order.model";
import { MaterialPricingType } from "../models/work-materials.model";

@Injectable({
    providedIn: 'root',
  })
export class PriceCalculatorService{
    calculateOrderCraftingComponentPrice(craftingComponent: OrderCraftingComponent): number {
        
        if(!craftingComponent.workMaterial?.relevantPrice) {
            return 0;
        }
        if(craftingComponent.workMaterial?.pricingType === MaterialPricingType.PerGram) {
            return (craftingComponent.workMaterialPrice * craftingComponent.quantity) || 0;
        }
        return craftingComponent.workMaterialPrice || 0
    }

    calculateOrderPrice(order: Order) {
        const componentsPrice = order.craftingComponents.map(cc => {
            const sign = cc.isDeposit ? -1: 1;
            return cc.totalComponentPrice * sign;
        })
            .reduce((sum, current) => sum + current, 0);
        return componentsPrice + order.laborPrice;
    }
}