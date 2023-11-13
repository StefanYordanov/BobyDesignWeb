import { Component, OnInit } from '@angular/core';
import { CustomerModel } from 'src/app/models/customers.model';
import { ModalFrameCallback } from 'src/app/models/modal-frame.model';
import { Order, OrderCraftingComponent } from 'src/app/models/order.model';
import { WorkMaterialModel } from 'src/app/models/work-materials.model';
import { CustomersService } from 'src/app/services/customers.service';
import { PriceCalculatorService } from 'src/app/services/price-calculator.service';


interface CraftingComponentFormCreationExtensions {
    craftingComponent: OrderCraftingComponentCreationModel,
    workMaterialTemp?: WorkMaterialModel,
    workMaterialCallback: ModalFrameCallback<WorkMaterialModel> 
    onRemove: () => void
}

interface OrderCreationModel {
    customer?: CustomerModel;
    description: string;
    finishingDate?: Date;
    craftingComponents: OrderCraftingComponentCreationModel[];
    laborPrice: number;
    totalPrice: number;
    deposit: number;
}

interface OrderCraftingComponentCreationModel {
    workMaterial?: WorkMaterialModel;
    workMaterialPrice: number;
    quantity: number;
    totalComponentPrice: number
}

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.scss'],
})
export class CreateOrderComponent implements OnInit {
  constructor(private customersService: CustomersService, private priceCalculatorService: PriceCalculatorService) {}

  craftingComponents: CraftingComponentFormCreationExtensions[]= [];

  newOrder: OrderCreationModel = {
    description: '',
    customer: undefined,
    craftingComponents: [],
    laborPrice: 0,
    totalPrice: 0,
    deposit: 0,
    finishingDate: undefined
  };
  customerTemp?: CustomerModel;
  customerCallback: ModalFrameCallback<CustomerModel> = {
    onOk: () => {
      this.newOrder.customer = this.customerTemp;
    },
  };

  finishingDateTemp?: Date;
  finishingDateCallback: ModalFrameCallback<Date> = {
    onOk: () => {
      this.newOrder.finishingDate = this.finishingDateTemp
    }
  }

  ngOnInit(): void {}

  addNewCraftingComponent() {
    const craftingComponent: OrderCraftingComponentCreationModel = {
        quantity: 0,
        totalComponentPrice: 0,
        workMaterialPrice: 0
    };

    const newEntry: CraftingComponentFormCreationExtensions = {
      craftingComponent,
      workMaterialTemp: undefined, 
      workMaterialCallback: {
        onOk: () => {
          if(newEntry.workMaterialTemp) {
            newEntry.craftingComponent.workMaterial = newEntry.workMaterialTemp,
            newEntry.craftingComponent.workMaterialPrice = newEntry.workMaterialTemp.relevantPrice?.sellingPrice || 0; 
          };
          this.triggerComponentRecalculation(newEntry.craftingComponent);
          console.log('TOWA!!!', this.craftingComponents, this.newOrder);
        }
      },
      onRemove: () => {
        this.craftingComponents = this.craftingComponents.filter(cc => cc.craftingComponent !== craftingComponent);
        this.newOrder.craftingComponents = this.newOrder.craftingComponents?.filter(cc => cc !== craftingComponent);
        this.triggerOrderTotalRecalculation();
      }
    }

    this.craftingComponents.push(newEntry)
    this.newOrder.craftingComponents.push(craftingComponent);
  }

  triggerComponentRecalculation(craftingComponent: OrderCraftingComponentCreationModel) {
    craftingComponent.totalComponentPrice = 
    this.priceCalculatorService.calculateOrderCraftingComponentPrice(<OrderCraftingComponent>(craftingComponent));    
    this.triggerOrderTotalRecalculation();
  }

  triggerOrderTotalRecalculation() {
    this.newOrder.totalPrice = this.priceCalculatorService.calculateOrderPrice(
      <Order>(this.newOrder)
    );
  }

  customerString(customer?:CustomerModel) {
    if(!customer) {
      return '';
    }
    return this.customersService.customerString(customer);
  }
}
