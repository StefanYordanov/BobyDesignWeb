
import { Injectable } from "@angular/core";
import { ApiService, Params } from "./api.service";
import { Order, OrderQuery } from "../models/order.model";

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
      const orders = await this.apiService.get<Order[]>('orders/getOrders', params);
      console.log(orders);
      return orders;
    }
}