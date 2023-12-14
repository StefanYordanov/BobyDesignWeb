import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CustomerModel } from 'src/app/models/customers.model';
import { Order, OrderStatus } from 'src/app/models/order.model';
import { CustomersService } from 'src/app/services/customers.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private customersService: CustomersService) { }
  order?: Order
  orderStatus = OrderStatus;

  ngOnInit(): void {
    this.activatedRoute.data.subscribe((response: any) => {
      this.order = response.order;
      this.order?.craftingComponents.map(cc => cc.workMaterial.name)
    });
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

}
