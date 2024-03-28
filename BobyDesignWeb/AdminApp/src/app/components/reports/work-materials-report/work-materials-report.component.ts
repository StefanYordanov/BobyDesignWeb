import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { DateOnlyModel, PageView } from 'src/app/models/common.model';
import { ModalFrameCallback } from 'src/app/models/modal-frame.model';
import { OrderItemsReport } from 'src/app/models/report.model';
import { WorkMaterialModel } from 'src/app/models/work-materials.model';
import { DateService } from 'src/app/services/date.service';
import { ReportsService } from 'src/app/services/reports.service';
import { WorkMaterialsService } from 'src/app/services/work-materials.service';

@Component({
  selector: 'app-work-materials-report',
  templateUrl: './work-materials-report.component.html',
  styleUrls: ['./work-materials-report.component.scss']
})
export class WorkMaterialsReportComponent implements OnInit {

orderItemsReport?: OrderItemsReport;
fromDate?: DateOnlyModel;
fromDateTemp?: DateOnlyModel;
fromDateCallback: ModalFrameCallback<Date> = {
  onOk: () => {
    this.fromDate = this.fromDateTemp
  }
}

toDate?: DateOnlyModel;
toDateTemp?: DateOnlyModel;
toDateCallback: ModalFrameCallback<Date> = {
  onOk: () => {
    this.toDate = this.toDateTemp
  }
}

status?: number;
type?: number;
paymentMethod?: number;

workMaterial?: WorkMaterialModel;
workMaterialTemp?: WorkMaterialModel;
workMaterialCallback: ModalFrameCallback<WorkMaterialModel> = {
  onOk: () => {
    this.workMaterial = this.workMaterialTemp;
  }
}

constructor(private activatedRoute: ActivatedRoute, 
  private workMaterialsService: WorkMaterialsService, private toastr: ToastrService,
   private reportsService: ReportsService, private router: Router, public dateService: DateService) { }



  async ngOnInit() {
    this.fromDate = this.activatedRoute.snapshot.queryParams["fromDate"] && this.dateService.stringToDateOnly(this.activatedRoute.snapshot.queryParams["fromDate"]);
    this.toDate = this.activatedRoute.snapshot.queryParams["toDate"] && this.dateService.stringToDateOnly(this.activatedRoute.snapshot.queryParams["toDate"]);
    this.status = this.activatedRoute.snapshot.queryParams["orderStatus"] && Number(this.activatedRoute.snapshot.queryParams["orderStatus"]);
    this.type = (this.activatedRoute.snapshot.queryParams["orderType"] && Number(this.activatedRoute.snapshot.queryParams["orderType"]));
    this.paymentMethod = (this.activatedRoute.snapshot.queryParams["orderPaymentMethod"] && Number(this.activatedRoute.snapshot.queryParams["orderPaymentMethod"]));
    if(this.activatedRoute.snapshot.queryParams["workMaterialId"]){
      this.workMaterial = await this.workMaterialsService.get(Number(this.activatedRoute.snapshot.queryParams["workMaterialId"])) || undefined;
    }

  }

  async applySearch() {

    if(!this.workMaterial?.id){
      this.toastr.error("Полето \"Материал\" е задължително");
      return;
    }
    const queryParams: Params = { 
      fromDate: this.fromDate && this.dateService.dateOnlyToString(this.fromDate),
      toDate: this.toDate && this.dateService.dateOnlyToString(this.toDate),
      workMaterialId: this.workMaterial?.id,
      status: this.status,
      type: this.type
  };
  this.router.navigate(
    [], 
    {
      relativeTo: this.activatedRoute,
      queryParams, 
      queryParamsHandling: 'merge', // remove to replace all query params by provided
    });
    this.orderItemsReport = await this.reportsService.getWorkMaterialReport({
      fromDate: this.fromDate && this.dateService.dateOnlyToString(this.fromDate),
      toDate: this.toDate && this.dateService.dateOnlyToString(this.toDate),
      workMaterialId: this.workMaterial!.id,
      orderType: this.type,
      orderStatus: this.status,
      orderPaymentMethod: this.paymentMethod
    
    }) || undefined;
  }

  async downloadExcelReport() {

    if(!this.workMaterial?.id){
      this.toastr.error("Полето \"Материал\" е задължително");
      return;
    }
    const queryParams: Params = { 
      fromDate: this.fromDate && this.dateService.dateOnlyToString(this.fromDate),
      toDate: this.toDate && this.dateService.dateOnlyToString(this.toDate),
      workMaterialId: this.workMaterial?.id,
      status: this.status,
      type: this.type
  };
  this.router.navigate(
    [], 
    {
      relativeTo: this.activatedRoute,
      queryParams, 
      queryParamsHandling: 'merge', // remove to replace all query params by provided
    });
    await this.reportsService.getWorkMaterialReportFile({
      fromDate: this.fromDate && this.dateService.dateOnlyToString(this.fromDate),
      toDate: this.toDate && this.dateService.dateOnlyToString(this.toDate),
      workMaterialId: this.workMaterial!.id,
      orderType: this.type,
      orderStatus: this.status,
      orderPaymentMethod: this.paymentMethod
    
    });
  }
}
