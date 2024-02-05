import { Injectable } from "@angular/core";
import { OrderCraftingComponent, Order } from "../models/order.model";
import { MaterialPricingType } from "../models/work-materials.model";

@Injectable({
    providedIn: 'root',
  })
export class PriceCalculatorService{
    

    calculateOrderCraftingComponentPrice(craftingComponent: OrderCraftingComponent): number {
        if(craftingComponent.workMaterial?.pricingType === MaterialPricingType.PerGram) {
            const unroundedPrice = (craftingComponent.workMaterialPrice * craftingComponent.quantity);
            
            const roundedPrice = Number(unroundedPrice.toFixed(2))
            console.log(unroundedPrice, roundedPrice)
            return roundedPrice || 0;
        }
        return craftingComponent.workMaterialPrice || 0
    }

    calculateOrderPrice(order: Order) {
        const componentsPrice = order.craftingComponents.map(cc => {
            const sign = cc.isDeposit ? -1: 1;
            return Number(cc.totalComponentPrice.toFixed(2)) * sign;
        })
            .reduce((sum, current) => sum + current, 0);
        return Number((componentsPrice + order.laborPrice).toFixed(2));
    }
}