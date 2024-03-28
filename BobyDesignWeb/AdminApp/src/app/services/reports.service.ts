
import { Injectable } from "@angular/core";
import { ApiService, Params } from "./api.service";
import { OrderItemsReport, WorkMaterialReportQuery } from "../models/report.model";

@Injectable({
  providedIn: 'root'
})
export class ReportsService{
    constructor(private apiService: ApiService) {

    }
    
    async getWorkMaterialReport(query: WorkMaterialReportQuery): Promise<OrderItemsReport | null> {

        let params: Params = {
          workMaterialId: query.workMaterialId
        };
        if(query.fromDate) {
          params['fromDate'] = query.fromDate;
        }
        if(query.toDate) {
          params['toDate'] = query.toDate;
        }
        if(query.orderStatus) {
          params['orderStatus'] = query.orderStatus;
        }
        if(query.orderType) {
          params['orderType'] = query.orderType;
        }
        if(query.orderPaymentMethod !== undefined) {
          params['orderPaymentMethod'] = query.orderPaymentMethod;
        }

        const workMaterialReport = await this.apiService.get<OrderItemsReport>('report/getWorkMaterialReport', params);
        console.log(workMaterialReport);
        return workMaterialReport;
      }

      async getWorkMaterialReportFile(query: WorkMaterialReportQuery): Promise<any> {

        let params = new URLSearchParams();
        for(let key in query){
          if((<any>query)[key] !== undefined) {
            params.set(key, (<any>query)[key]) 
          }
          
        }

        const downloadLink = document.createElement('a');
        downloadLink.href = 'report/getWorkMaterialReportFile?' +params.toString();
        console.log(downloadLink.href)
        document.body.appendChild(downloadLink);
        downloadLink.click();

      }
}