import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { PageView } from 'src/app/models/common.model';
import { Order } from 'src/app/models/order.model';
import { OrdersService } from 'src/app/services/orders.service';

@Component({
  selector: 'app-orders-view',
  templateUrl: './orders-view.component.html',
  styleUrls: ['./orders-view.component.scss']
})
export class OrdersViewComponent implements OnInit {

ordersPage?: PageView<Order>;
searchPhrase: string = '';

constructor(private activatedRoute: ActivatedRoute, private router: Router, private ordersService: OrdersService) { }



  ngOnInit(): void {
    this.activatedRoute.data.subscribe((response: any) => {
      this.ordersPage = response.ordersPage;
    });
    
    this.searchPhrase = this.activatedRoute.snapshot.queryParams["searchPhrase"];
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
      searchPhrase: this.searchPhrase
  };
  this.router.navigate(
    [], 
    {
      relativeTo: this.activatedRoute,
      queryParams, 
      queryParamsHandling: 'merge', // remove to replace all query params by provided
    });
    this.ordersPage = await this.ordersService.getOrdersPagination({searchPhrase: this.searchPhrase},0) || undefined;
  }
}
