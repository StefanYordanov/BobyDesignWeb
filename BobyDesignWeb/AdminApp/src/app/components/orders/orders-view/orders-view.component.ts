import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { DateOnlyModel, PageView } from 'src/app/models/common.model';
import { CustomerModel } from 'src/app/models/customers.model';
import { ModalFrameCallback } from 'src/app/models/modal-frame.model';
import { Order } from 'src/app/models/order.model';
import { CustomersService } from 'src/app/services/customers.service';
import { DateService } from 'src/app/services/date.service';
import { OrdersService } from 'src/app/services/orders.service';

@Component({
  selector: 'app-orders-view',
  templateUrl: './orders-view.component.html',
  styleUrls: ['./orders-view.component.scss']
})
export class OrdersViewComponent implements OnInit {

ordersPage?: PageView<Order>;
searchPhrase: string = '';
fromDate?: DateOnlyModel;
fromDateTemp?: DateOnlyModel;
fromDateCallback: ModalFrameCallback<Date> = {
  onOk: () => {
    this.fromDate = this.fromDateTemp
  }
}

toDate?: DateOnlyModel;
toDateTemp?: DateOnlyModel;
toDateCallback: ModalFrameCallback<Date> = {
  onOk: () => {
    this.toDate = this.toDateTemp
  }
}

status?: number;

customer?: CustomerModel;
customerTemp?: CustomerModel;
customerCallback: ModalFrameCallback<CustomerModel> = {
  onOk: () => {
    this.customer = this.customerTemp;
  }
}

constructor(private activatedRoute: ActivatedRoute, public customersService: CustomersService,
   private router: Router, private ordersService: OrdersService, public dateService: DateService) { }



  async ngOnInit() {
    this.activatedRoute.data.subscribe((response: any) => {
      this.ordersPage = response.ordersPage;
    });
    
    this.searchPhrase = this.activatedRoute.snapshot.queryParams["searchPhrase"];
    this.fromDate = this.activatedRoute.snapshot.queryParams["fromDate"] && this.dateService.stringToDateOnly(this.activatedRoute.snapshot.queryParams["fromDate"]);
    this.toDate = this.activatedRoute.snapshot.queryParams["toDate"] && this.dateService.stringToDateOnly(this.activatedRoute.snapshot.queryParams["toDate"]);
    this.status = this.activatedRoute.snapshot.queryParams["status"] && Number(this.activatedRoute.snapshot.queryParams["status"]);
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
      fromDate: this.fromDate && this.dateService.dateOnlyToString(this.fromDate),
      toDate: this.toDate && this.dateService.dateOnlyToString(this.toDate),
      customerId: this.customer?.id,
      status: this.status
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
      fromDate: this.fromDate && this.dateService.dateOnlyToString(this.fromDate),
      toDate: this.toDate && this.dateService.dateOnlyToString(this.toDate),
      customerId: this.customer?.id,
      status: this.status
    
    },0) || undefined;
  }

  customerString(customer?:CustomerModel) {
    if(!customer) {
      return '';
    }
    return this.customersService.customerString(customer);
  }
}
