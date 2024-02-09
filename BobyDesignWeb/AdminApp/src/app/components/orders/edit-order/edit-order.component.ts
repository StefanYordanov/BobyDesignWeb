import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { DrawingCanvasComponent } from 'src/app/drawing-canvas/drawing-canvas.component';
import { DateOnlyModel } from 'src/app/models/common.model';
import { CustomerModel } from 'src/app/models/customers.model';
import { JewelryShopModel } from 'src/app/models/jewelry-shop.model';
import { ModalFrameCallback } from 'src/app/models/modal-frame.model';
import {
  Order,
  OrderCraftingComponent,
  OrderPaymentMethod,
  OrderStatus,
} from 'src/app/models/order.model';
import { User } from 'src/app/models/user.model';
import { CanvasBackground } from 'src/app/models/web-content.model';
import { WorkMaterialModel } from 'src/app/models/work-materials.model';
import { BlobService } from 'src/app/services/blob.service';
import { CustomersService } from 'src/app/services/customers.service';
import { DateService } from 'src/app/services/date.service';
import { JewelryShopsService } from 'src/app/services/jewelry-shops.service';
import { OrdersService } from 'src/app/services/orders.service';
import { PriceCalculatorService } from 'src/app/services/price-calculator.service';

interface CraftingComponentFormCreationExtensions {
  craftingComponent: OrderCraftingComponentCreationModel;
  workMaterialTemp?: WorkMaterialModel;
  workMaterialCallback: ModalFrameCallback<WorkMaterialModel>;
  onRemove: () => void;
}

interface OrderEditModel {
  id: number;
  updateImage: boolean;
  customer: CustomerModel;
  description: string;
  finishingDate: DateOnlyModel;
  craftingComponents: OrderCraftingComponentCreationModel[];
  deletedCraftingComponentIds: number[];
  laborPrice: number;
  totalPrice: number;
  deposit: number;
  createdOn: Date;
  shop: JewelryShopModel;
  shopUser: User;
  status: OrderStatus;
  paymentMethod: OrderPaymentMethod;
  imageFileName: string;
}

interface OrderCraftingComponentCreationModel {
  id?: number;
  workMaterial?: WorkMaterialModel;
  isDeposit: boolean;
  workMaterialPrice: number;
  quantity: number;
  totalComponentPrice: number;
}

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html',
  styleUrls: ['./edit-order.component.scss'],
})
export class EditOrderComponent implements OnInit {
  constructor(
    private customersService: CustomersService,
    private priceCalculatorService: PriceCalculatorService,
    private ordersService: OrdersService,
    private toastr: ToastrService,
    private blobService: BlobService,
    private router: Router,
    private jewelryShopsService: JewelryShopsService,
    private activatedRoute: ActivatedRoute,
    public dateService: DateService
  ) {}

  craftingComponents: CraftingComponentFormCreationExtensions[] = [];
  @ViewChild('drawingCanvas') drawingCanvas!: DrawingCanvasComponent;

  canvasCallback: ModalFrameCallback<string> = {
    onOk: () => {
      this.base64PngContent =
        this.drawingCanvas.getCanvasAsBase64String() || '';
        if(this.editOrder) {
          this.editOrder.updateImage = true;
        }
    },
  };
  base64PngContent?: string;
  initialBackground?: CanvasBackground;
  editOrder?: OrderEditModel;
  customerTemp?: CustomerModel;
  customerCallback: ModalFrameCallback<CustomerModel> = {
    onOk: () => {
      if (this.editOrder && this.customerTemp) {
        this.editOrder.customer = this.customerTemp;
      }
    },
  };

  finishingDateTemp?: DateOnlyModel;
  finishingDateCallback: ModalFrameCallback<DateOnlyModel> = {
    onOk: () => {
      if (this.editOrder && this.finishingDateTemp) {
        this.editOrder.finishingDate = this.finishingDateTemp;
      }
    },
  };

  ngOnInit(): void {
    this.activatedRoute.data.subscribe((response: any) => {
      const order: Order = response.order;
      this.editOrder = {
        id: order.id,
        updateImage: false,
        customer: order.customer,
        finishingDate: order.finishingDate,
        description: order.description,
        deletedCraftingComponentIds: [],
        craftingComponents: order.craftingComponents,
        laborPrice: order.laborPrice,
        deposit: order.deposit,
        totalPrice: order.totalPrice,
        createdOn: order.createdOn,
        shop: order.shop,
        shopUser: order.shopUser,
        status: order.status,
        paymentMethod: order.paymentMethod,
        imageFileName: order.imageFileName,
      };

      this.imageUrlToBase64(order.imageFileName).then((fileContent) => {
        this.base64PngContent = fileContent;
      });

      this.initialBackground = {
        name: 'initial.png',
        url: order.imageFileName,
      };

      for (let i = 0, len = order.craftingComponents.length; i < len; i++) {
        const orderComponent = order.craftingComponents[i];
        const craftingComponent: OrderCraftingComponentCreationModel = orderComponent;
        
        const newEntry: CraftingComponentFormCreationExtensions = {
          craftingComponent,
          workMaterialTemp: undefined,
          workMaterialCallback: {
            onOk: () => {
              if (newEntry.workMaterialTemp) {
                (newEntry.craftingComponent.workMaterial =
                  newEntry.workMaterialTemp),
                  console.log(
                    'WOrk material, ',
                    newEntry.craftingComponent.workMaterial
                  );
                newEntry.craftingComponent.workMaterialPrice =
                  (craftingComponent.isDeposit
                    ? newEntry.workMaterialTemp.relevantPrice?.purchasingPrice
                    : newEntry.workMaterialTemp.relevantPrice?.sellingPrice) ||
                  0;
              }
              this.triggerComponentRecalculation(newEntry.craftingComponent);
            },
          },
          onRemove: () => {
            if (!this.editOrder) {
              return;
            }
            this.craftingComponents = this.craftingComponents.filter(
              (cc) => cc.craftingComponent !== craftingComponent
            );
            this.editOrder.craftingComponents =
              this.editOrder.craftingComponents?.filter(
                (cc) => cc !== craftingComponent
              );
            this.editOrder.deletedCraftingComponentIds.push(craftingComponent.id!);
            this.triggerOrderTotalRecalculation();
          },
        };

        this.craftingComponents.push(newEntry);
      }
    });
  }

  imageUrlToBase64 = async (url: string): Promise<string> => {
    const data = await fetch(url);
    const blob = await data.blob();
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(blob);
      reader.onloadend = () => {
        const base64data = reader.result?.toString() || '';
        resolve(base64data);
      };
      reader.onerror = reject;
    });
  };

  addNewCraftingComponent(isDeposit = false) {
    if (!this.editOrder) {
      return;
    }

    const craftingComponent: OrderCraftingComponentCreationModel = {
      quantity: 0,
      isDeposit,
      totalComponentPrice: 0,
      workMaterialPrice: 0,
    };

    const newEntry: CraftingComponentFormCreationExtensions = {
      craftingComponent,
      workMaterialTemp: undefined,
      workMaterialCallback: {
        onOk: () => {
          if (newEntry.workMaterialTemp) {
            (newEntry.craftingComponent.workMaterial =
              newEntry.workMaterialTemp),
              console.log(
                'WOrk material, ',
                newEntry.craftingComponent.workMaterial
              );
            newEntry.craftingComponent.workMaterialPrice =
              (isDeposit
                ? newEntry.workMaterialTemp.relevantPrice?.purchasingPrice
                : newEntry.workMaterialTemp.relevantPrice?.sellingPrice) || 0;
          }
          this.triggerComponentRecalculation(newEntry.craftingComponent);
        },
      },
      onRemove: () => {
        if (!this.editOrder) {
          return;
        }
        this.craftingComponents = this.craftingComponents.filter(
          (cc) => cc.craftingComponent !== craftingComponent
        );
        this.editOrder.craftingComponents =
          this.editOrder.craftingComponents?.filter(
            (cc) => cc !== craftingComponent
          );
        this.triggerOrderTotalRecalculation();
      },
    };

    this.craftingComponents.push(newEntry);
    this.editOrder.craftingComponents.push(craftingComponent);
  }

  getCraftingComponents(isDeposit = false) {
    return this.craftingComponents.filter(
      (cc) => cc.craftingComponent.isDeposit === isDeposit
    );
  }

  triggerComponentRecalculation(
    craftingComponent: OrderCraftingComponentCreationModel
  ) {
    craftingComponent.totalComponentPrice =
      this.priceCalculatorService.calculateOrderCraftingComponentPrice(
        <OrderCraftingComponent>craftingComponent
      );
    this.triggerOrderTotalRecalculation();
  }

  triggerOrderTotalRecalculation() {
    if (!this.editOrder) {
      return;
    }
    this.editOrder.totalPrice = this.priceCalculatorService.calculateOrderPrice(
      <Order>this.editOrder
    );
  }

  workMaterialsString(): string {
    return (
      this.editOrder?.craftingComponents
        .filter((cc) => !cc.isDeposit && cc.workMaterial)
        .map((cc) => cc.workMaterial!.name)
        .join(', ') || ''
    );
  }

  getDepositComponentsString() {
    return (
      this.editOrder?.craftingComponents
        .filter((cc) => cc.isDeposit && cc.workMaterial)
        .map(
          (cc) =>
            `${cc.workMaterial!.name}(${cc.quantity} ${
              cc.workMaterial!.measuringUnit
            })`
        )
        .join(', ') || ''
    );
  }

  customerString(customer?: CustomerModel) {
    if (!customer) {
      return '';
    }
    return this.customersService.customerString(customer);
  }

  async confirmEditOrder() {
    if (!this.editOrder) {
      this.toastr.error('Няма заредена поръчка');
      return;
    }
    if (
      this.editOrder.craftingComponents.some(
        (cc) => cc.workMaterial === undefined
      )
    ) {
      this.toastr.error('Има невъведени материали сред компонентите');
      return;
    }
    const shop = await this.jewelryShopsService.getUserActiveShop();
    if (!shop) {
      this.toastr.error('Потребителят няма зададен магазин');
      return;
    }

    const response = await this.ordersService.updateOrder({
      sketchBlob: (this.editOrder.updateImage && this.base64PngContent)
        ? this.blobService.dataURIToBlob(this.base64PngContent)
        : undefined,
      deletedCraftingComponentIds: this.editOrder.deletedCraftingComponentIds,
      model: {
        ...this.editOrder,
        id: this.editOrder.id,
        customer: this.editOrder.customer,
        finishingDate: this.editOrder.finishingDate,
        deposit: this.editOrder.deposit || 0,
        shop: this.editOrder.shop,
        imageFileName: '',
        craftingComponents: this.editOrder.craftingComponents.map((cc) => {
          return { ...cc, id: cc.id || 0, workMaterial: cc.workMaterial! };
        }),
        shopUser: {
          id: '',
          firstName: '',
          lastName: '',
          phoneNumber: '',
          email: '',
          userName: '',
        },
        createdOn: this.editOrder.createdOn,
        status: this.editOrder.status,
      },
    });

    if (response) {
      this.toastr.success('Успешно създадохте поръчка');
      console.log(response);
      this.router.navigateByUrl('orders/details?orderId=' + response.id);
    }
  }

  log() {
    console.log(this.editOrder, this.craftingComponents)
  }
}
