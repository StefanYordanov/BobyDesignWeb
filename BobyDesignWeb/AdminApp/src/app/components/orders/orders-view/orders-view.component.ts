import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { PageView } from 'src/app/models/common.model';
import { CustomerModel } from 'src/app/models/customers.model';
import { ModalFrameCallback } from 'src/app/models/modal-frame.model';
import { Order } from 'src/app/models/order.model';
import { CustomersService } from 'src/app/services/customers.service';
import { OrdersService } from 'src/app/services/orders.service';

@Component({
  selector: 'app-orders-view',
  templateUrl: './orders-view.component.html',
  styleUrls: ['./orders-view.component.scss']
})
export class OrdersViewComponent implements OnInit {

ordersPage?: PageView<Order>;
searchPhrase: string = '';
fromDate?: Date;
fromDateTemp?: Date;
fromDateCallback: ModalFrameCallback<Date> = {
  onOk: () => {
    this.fromDate = this.fromDateTemp
  }
}

toDate?: Date;
toDateTemp?: Date;
toDateCallback: ModalFrameCallback<Date> = {
  onOk: () => {
    this.toDate = this.toDateTemp
  }
}

customer?: CustomerModel;
customerTemp?: CustomerModel;
customerCallback: ModalFrameCallback<CustomerModel> = {
  onOk: () => {
    this.customer = this.customerTemp;
  }
}

constructor(private activatedRoute: ActivatedRoute, private customersService: CustomersService,
   private router: Router, private ordersService: OrdersService) { }



  async ngOnInit() {
    this.activatedRoute.data.subscribe((response: any) => {
      this.ordersPage = response.ordersPage;
    });
    
    this.searchPhrase = this.activatedRoute.snapshot.queryParams["searchPhrase"];
    this.fromDate = this.activatedRoute.snapshot.queryParams["fromDate"] && new Date(this.activatedRoute.snapshot.queryParams["fromDate"]);
    this.toDate = this.activatedRoute.snapshot.queryParams["toDate"] && new Date(this.activatedRoute.snapshot.queryParams["toDate"])
    if(this.activatedRoute.snapshot.queryParams["customerId"]){
      this.customer = await this.customersService.getCustomer(Number(this.activatedRoute.snapshot.queryParams["customerId"])) || undefined;
    }

  }

  async switchPage(pageNumber: number) {
    const queryParams: Params = { 
      page: pageNumber
  };
  this.router.navigate(
    [], 
    {
      relativeTo: this.activatedRoute,
      queryParams, 
      queryParamsHandling: 'merge', // remove to replace all query params by provided
    });
    this.ordersPage = await this.ordersService.getOrdersPagination({
      searchPhrase: this.searchPhrase}, pageNumber) || undefined;
  }

  async applySearch() {
    const queryParams: Params = { 
      searchPhrase: this.searchPhrase,
      fromDate: this.fromDate,
      toDate: this.toDate,
      customerId: this.customer?.id
  };
  this.router.navigate(
    [], 
    {
      relativeTo: this.activatedRoute,
      queryParams, 
      queryParamsHandling: 'merge', // remove to replace all query params by provided
    });
    this.ordersPage = await this.ordersService.getOrdersPagination({
      searchPhrase: this.searchPhrase, 
      fromDate: this.fromDate?.toDateString(),
      toDate: this.toDate?.toDateString(),
      customerId: this.customer?.id
    
    },0) || undefined;
  }

  customerString(customer?:CustomerModel) {
    if(!customer) {
      return '';
    }
    return this.customersService.customerString(customer);
  }
}
