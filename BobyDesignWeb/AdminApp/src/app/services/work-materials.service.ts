
import { Injectable } from "@angular/core";
import { WorkMaterialModel } from "../models/work-materials.model";
import { ApiService, Params } from "./api.service";

@Injectable({
  providedIn: 'root'
})
export class WorkMaterialsService{
    constructor(private apiService: ApiService) {

    }

    async editWorkMaterial(model: WorkMaterialModel): Promise<WorkMaterialModel | null> {
      const response = await this.apiService.post<WorkMaterialModel>('workMaterials/editWorkMaterial', model)
      console.log(response);
      return response;
    }

    async addNewWorkMaterial(model: WorkMaterialModel): Promise<WorkMaterialModel | null> {
      const response = await this.apiService.post<WorkMaterialModel>('workMaterials/insertWorkMaterial', model)
      console.log(response);
      return response;
    }
    
    async getAllWorkMaterials(pricingDate?: Date): Promise<WorkMaterialModel[] | null> {

        let params: Params = {
        };

        if(pricingDate) {
          params['pricingDate'] = pricingDate.toDateString();
        }

        const workMaterials = await this.apiService.get<WorkMaterialModel[]>('workMaterials/getAll', params);
        console.log(workMaterials);
        return workMaterials;
      }

      async getBySearch(search: string, pricingDate?: Date): Promise<WorkMaterialModel[] | null> {

        let params: Params = {
          search: search,
        };
        
        if(pricingDate) {
          params['pricingDate'] = pricingDate.toDateString();
        }
        const workMaterials = await this.apiService.get<WorkMaterialModel[]>('workMaterials/getBySearch', params);
        console.log(workMaterials);
        return workMaterials;
      }

      async get(id: number): Promise<WorkMaterialModel | null> {

        let params: Params = {
          id: id
        };
        const workMaterials = await this.apiService.get<WorkMaterialModel>('workMaterials/get', params);
        console.log(workMaterials);
        return workMaterials;
      }
}