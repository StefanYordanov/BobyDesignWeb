
import { Injectable } from "@angular/core";
import { ApiService, Params } from "./api.service";
import { Order, OrderQuery, PayOrderQuery, SubmitOrder } from "../models/order.model";
import { PageView } from "../models/common.model";

@Injectable({
  providedIn: 'root'
})
export class OrdersService{
    constructor(private apiService: ApiService) {

    }

    async createOrder(order: SubmitOrder): Promise<Order | null>{
      const formData = new FormData();
      if(order.sketchBlob) {
        formData.append('sketchBlob', order.sketchBlob)
      }
      formData.append('model', JSON.stringify(order.model))
      
      const response = await this.apiService.postFormData<Order>(
        'orders/insertOrder',
        formData
      );
      console.log(response);
      return response;
    }

    async payOrder(query: PayOrderQuery) : Promise<Order| null> {
      const response = await this.apiService.post<Order>('orders/pay', query) 
      return response;
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

    async getOrder(orderId: number): Promise<Order | null> {
      let params: Params = {
      };
      params['orderId'] = orderId;
      
      const order = await this.apiService.get<Order>('orders/getOrder', params);
      console.log(order);
      return order;
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