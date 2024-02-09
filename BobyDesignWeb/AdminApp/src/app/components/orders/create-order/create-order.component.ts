import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { DrawingCanvasComponent } from 'src/app/drawing-canvas/drawing-canvas.component';
import { DateOnlyModel } from 'src/app/models/common.model';
import { CustomerModel } from 'src/app/models/customers.model';
import { ModalFrameCallback } from 'src/app/models/modal-frame.model';
import { Order, OrderCraftingComponent, OrderPaymentMethod, OrderStatus } from 'src/app/models/order.model';
import { WorkMaterialModel } from 'src/app/models/work-materials.model';
import { BlobService } from 'src/app/services/blob.service';
import { CustomersService } from 'src/app/services/customers.service';
import { DateService } from 'src/app/services/date.service';
import { JewelryShopsService } from 'src/app/services/jewelry-shops.service';
import { OrdersService } from 'src/app/services/orders.service';
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
    finishingDate?: DateOnlyModel;
    craftingComponents: OrderCraftingComponentCreationModel[];
    laborPrice: number;
    totalPrice: number;
    deposit: number;
    paymentMethod: OrderPaymentMethod
}

interface OrderCraftingComponentCreationModel {
    workMaterial?: WorkMaterialModel;
    isDeposit: boolean;
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
  constructor(private customersService: CustomersService, 
    private priceCalculatorService: PriceCalculatorService, 
    private ordersService: OrdersService,
    private toastr: ToastrService,
    private blobService: BlobService, private router: Router, 
    private jewelryShopsService: JewelryShopsService, public dateService: DateService) {
      
    }

  craftingComponents: CraftingComponentFormCreationExtensions[]= [];
  @ViewChild('drawingCanvas') drawingCanvas!: DrawingCanvasComponent;
  
  canvasCallback: ModalFrameCallback<string> = {
    onOk: () => {
      this.base64PngContent = this.drawingCanvas.getCanvasAsBase64String() || '';
      console.log(this.base64PngContent);
    }
  }
  base64PngContent?: string;

  paymentMethodEnum = OrderPaymentMethod;

  newOrder: OrderCreationModel = {
    description: '',
    customer: undefined,
    craftingComponents: [],
    laborPrice: 0,
    totalPrice: 0,
    deposit: 0,
    finishingDate: undefined,
    paymentMethod: OrderPaymentMethod.Cash
  };
  customerTemp?: CustomerModel;
  customerCallback: ModalFrameCallback<CustomerModel> = {
    onOk: () => {
      this.newOrder.customer = this.customerTemp;
    },
  };

  finishingDateTemp?: DateOnlyModel;
  finishingDateCallback: ModalFrameCallback<DateOnlyModel> = {
    onOk: () => {
      this.newOrder.finishingDate = this.finishingDateTemp
    }
  }

  ngOnInit(): void {}

  addNewCraftingComponent(isDeposit = false) {
    const craftingComponent: OrderCraftingComponentCreationModel = {
        quantity: 0,
        isDeposit,
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
            console.log('WOrk material, ', newEntry.craftingComponent.workMaterial)
            newEntry.craftingComponent.workMaterialPrice = (isDeposit ? newEntry.workMaterialTemp.relevantPrice?.purchasingPrice : newEntry.workMaterialTemp.relevantPrice?.sellingPrice) || 0; 
          };
          this.triggerComponentRecalculation(newEntry.craftingComponent);
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

  getCraftingComponents(isDeposit = false) {
    return this.craftingComponents.filter(cc => cc.craftingComponent.isDeposit === isDeposit);
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

  workMaterialsString(): string{
    return this.newOrder?.craftingComponents.filter(cc => !cc.isDeposit && cc.workMaterial).map(cc => cc.workMaterial!.name).join(', ') || ''
  }

  getDepositComponentsString() {
    return this.newOrder?.craftingComponents.filter(cc => cc.isDeposit && cc.workMaterial)
      .map(cc=> `${cc.workMaterial!.name}(${cc.quantity} ${cc.workMaterial!.measuringUnit})`).join(', ') || '';
  }

  customerString(customer?:CustomerModel) {
    if(!customer) {
      return '';
    }
    return this.customersService.customerString(customer);
  }

  async createOrder() {
    if(!this.newOrder.customer) {
      this.toastr.error('Въведете клиент')
      return;
    }
    if(!this.newOrder.finishingDate) {
      this.toastr.error('Въведете дата на завършване')
      return;
    }
    if(this.newOrder.craftingComponents.some(cc => cc.workMaterial === undefined)) {
      this.toastr.error('Има невъведени материали сред компонентите')
      return;
    }
    const shop = await this.jewelryShopsService.getUserActiveShop();
    if(!shop) {
      this.toastr.error('Потребителят няма зададен магазин')
      return;
    }

    const response = await this.ordersService.createOrder({ 
      sketchBlob: this.base64PngContent ? this.blobService.dataURIToBlob(this.base64PngContent) : undefined, 
      model: { ...this.newOrder, id: 0, 
        customer: this.newOrder.customer,
        finishingDate: this.newOrder.finishingDate,
        deposit: this.newOrder.deposit || 0,
        shop: shop,
        imageFileName: '',
        craftingComponents: this.newOrder.craftingComponents.map(cc => {
          return {...cc, id: 0, workMaterial: cc.workMaterial!}
        }),
        shopUser: { id: '', firstName: '', lastName: '', phoneNumber: '', email: '', userName: '' },
        createdOn: new Date(),
        status: OrderStatus.Opened,
        paymentMethod: this.newOrder.paymentMethod
      
      }
    });

    if(response) {
      this.toastr.success('Успешно създадохте поръчка');
      console.log(response);
      this.router.navigateByUrl('orders/details?orderId='+response.id);
      
    }
    

  }
}
