import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CustomerModel } from 'src/app/models/customers.model';
import { ModalFrameCallback } from 'src/app/models/modal-frame.model';
import { Order, OrderPaymentMethod, OrderStatus } from 'src/app/models/order.model';
import { CustomersService } from 'src/app/services/customers.service';
import { DateService } from 'src/app/services/date.service';
import { OrdersService } from 'src/app/services/orders.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private customersService: CustomersService, 
    private ordersService: OrdersService, public dateService: DateService) { }
  order?: Order
  orderStatus = OrderStatus;

  finishOrderCallback: ModalFrameCallback<null> = {
    onOk: async () => {
      if(!this.order) {
        return;
      }
      const response = await this.ordersService.payOrder({orderId: this.order.id, payment: this.order.totalPrice - this.order.deposit})
      if(response) {
        this.order = response;
      }
    }
  }
  ngOnInit(): void {
    this.activatedRoute.data.subscribe((response: any) => {
      this.order = response.order;
    });
    this.activatedRoute.queryParamMap.subscribe(params => {
      const orderId = Number(params.get('orderId'));
      this.ordersService.getOrder(orderId).then(o => {
        this.order = o || undefined;
      });
    })
  }

  getCraftingComponents(isDeposit = false) {
    return this.order?.craftingComponents.filter(cc => cc.isDeposit === isDeposit);
  }

  customerString(customer?:CustomerModel) {
    if(!customer) {
      return '';
    }
    return this.customersService.customerString(customer);
  }

  paymentMethodString(){
    if(!this.order) {
      return ''
    }
    if(this.order.paymentMethod === OrderPaymentMethod.Cash) {
      return 'В Брой'
    }
    if(this.order.paymentMethod === OrderPaymentMethod.Card) {
      return 'С карта'
    }
    return ''
  }

}
