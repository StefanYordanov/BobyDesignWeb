
import { Injectable } from "@angular/core";
import { ApiService, Params } from "./api.service";
import { Order, OrderQuery } from "../models/order.model";
import { PageView } from "../models/common.model";

@Injectable({
  providedIn: 'root'
})
export class OrdersService{
    constructor(private apiService: ApiService) {

    }

    async getOrders(query: OrderQuery): Promise<Order[] | null> {
      let params: Params = {
      };

      if(query.fromDate) {
        params['fromDate'] = query.fromDate;
      }
      if(query.toDate) {
        params['toDate'] = query.toDate;
      }
      if(query.searchPhrase) {
        params['searchPhrase'] = query.searchPhrase;
      }
      if(query.customerId) {
        params['customerId'] = query.customerId;
      }
      const orders = await this.apiService.get<Order[]>('orders/getOrders', params);
      console.log(orders);
      return orders;
    }

    async getOrdersPagination(query: OrderQuery, page?: number): Promise<PageView<Order> | null> {
      let params: Params = {
      };

      if(query.fromDate) {
        params['fromDate'] = query.fromDate;
      }
      if(query.toDate) {
        params['toDate'] = query.toDate;
      }
      if(query.searchPhrase) {
        params['searchPhrase'] = query.searchPhrase;
      }
      if(query.customerId) {
        params['customerId'] = query.customerId;
      }
      if(page) {
        params['page'] = page
      }
      const orders = await this.apiService.get<PageView<Order>>('orders/getOrdersPagination', params);
      console.log(orders);
      return orders;
    }
}