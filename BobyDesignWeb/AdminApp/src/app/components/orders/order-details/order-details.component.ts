import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CustomerModel } from 'src/app/models/customers.model';
import { Order } from 'src/app/models/order.model';
import { CustomersService } from 'src/app/services/customers.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private customersService: CustomersService) { }
  order?: Order

  ngOnInit(): void {
    this.activatedRoute.data.subscribe((response: any) => {
      this.order = response.order;
    });
  }

  customerString(customer?:CustomerModel) {
    if(!customer) {
      return '';
    }
    return this.customersService.customerString(customer);
  }

}
