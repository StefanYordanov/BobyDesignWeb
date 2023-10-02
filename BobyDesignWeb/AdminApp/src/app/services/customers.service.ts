import { Injectable } from '@angular/core';
import { ApiService, Params } from './api.service';
import { CustomerModel } from '../models/customers.model';
import { PageView } from '../models/common.model';

@Injectable({
  providedIn: 'root',
})
export class CustomersService {
  constructor(private apiService: ApiService) {}

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
    const workMaterials = await this.apiService.get<PageView<CustomerModel>>(
      'customers/search',
      params
    );
    console.log(workMaterials);
    return workMaterials;
  }
}
