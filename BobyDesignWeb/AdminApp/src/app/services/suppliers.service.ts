import { Injectable } from "@angular/core";
import { ApiService, Params } from "./api.service";
import { SupplierModel } from "../models/supplier.model";

@Injectable({
    providedIn: 'root'
  })
  export class SuppliersService {
      constructor(private apiService: ApiService) {
  
      }
  
      all?: SupplierModel[];
      active?: SupplierModel

      async getAll(): Promise<SupplierModel[] | null> {
        if(this.all !== undefined) {
            return this.all;
        }
        const response = await this.apiService.get<SupplierModel[]>('suppliers/getAll')
        this.all = response || undefined;
        return response;
      }
  
      async getDefaultActiveSupplier(): Promise<SupplierModel | null> {
        if(this.active !== undefined) {
            return this.active;
        }
        const response = await this.apiService.get<SupplierModel>('suppliers/getDefaultActiveSupplier')
        this.active = response || undefined;
        return response;
      }

      setActiveSupplier(id: number | null) {
        this.active = this.all?.filter(a => a.id === id)[0];
      }
    }