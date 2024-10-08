import { Injectable } from '@angular/core';
import { ApiService, Params } from './api.service';
import { CustomerModel, CustomerReviewModel } from '../models/customers.model';
import { PageView } from '../models/common.model';

@Injectable({
  providedIn: 'root',
})
export class CustomersService {
  constructor(private apiService: ApiService) {}

  customerString(customer?:CustomerModel) {
    if(!customer) {
      return ''
    }
    return [customer.name, customer.phoneNumber, customer.email]
    .filter(customerEl=> customerEl && customerEl.trim().length).join('/');
  }

  async editCustomer(model: CustomerModel): Promise<CustomerModel | null> {
    const response = await this.apiService.post<CustomerModel>(
      'customers/edit',
      model
    );
    console.log(response);
    return response;
  }

  async addNewCustomer(model: CustomerModel): Promise<CustomerModel | null> {
    const response = await this.apiService.post<CustomerModel>(
      'customers/create',
      model
    );
    console.log(response);
    return response;
  }

  async addNewCustomerReview(model: CustomerReviewModel): Promise<CustomerReviewModel | null> {
    const response = await this.apiService.post<CustomerReviewModel>(
      'customers/createReview',
      model
    );
    console.log(response);
    return response;
  }

  async getCustomer(id: number): Promise<CustomerModel | null> {
    let params: Params = {
      id,
    };
    const customer = await this.apiService.get<CustomerModel>(
      'customers/getCustomer',
      params
    );
    console.log(customer);
    return customer;
  }

  async getCustomers(
    page: number,
    searchPhrase?: string
  ): Promise<PageView<CustomerModel> | null> {
    console.log({ page, searchPhrase });
    let params: Params = {
      page,
    };
    if (searchPhrase) {
      params['searchPhrase'] = searchPhrase;
    }
    const customersPage = await this.apiService.get<PageView<CustomerModel>>(
      'customers/search',
      params
    );
    console.log(customersPage);
    return customersPage;
  }
}
