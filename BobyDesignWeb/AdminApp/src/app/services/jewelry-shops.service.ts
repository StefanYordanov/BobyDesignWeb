import { Injectable } from "@angular/core";
import { ApiService, Params } from "./api.service";
import { JewelryShopModel } from "../models/jewelry-shop.model";


@Injectable({
    providedIn: 'root'
  })
  export class JewelryShopsService{
      constructor(private apiService: ApiService) {
  
      }
  
      all?: JewelryShopModel[];
      active?: JewelryShopModel

      async getAll(): Promise<JewelryShopModel[] | null> {
        if(this.all !== undefined) {
            return this.all;
        }
        const response = await this.apiService.get<JewelryShopModel[]>('jewelryShops/getAll')
        this.all = response || undefined;
        return response;
      }
  
      async getUserActiveShop(): Promise<JewelryShopModel | null> {
        if(this.active !== undefined) {
            return this.active;
        }
        const response = await this.apiService.get<JewelryShopModel>('jewelryShops/getUserActiveShop')
        this.active = response || undefined;
        return response;
      }

      async setUserActiveShop(id: number | null) {
        let params: Params = {
        };
  
        if(id) {
          params['id'] = id;
        }
        const response = await this.apiService.post<JewelryShopModel>('jewelryShops/setUserActiveShop', {}, params)
        this.active = response || undefined;
        return response;
      }
    }