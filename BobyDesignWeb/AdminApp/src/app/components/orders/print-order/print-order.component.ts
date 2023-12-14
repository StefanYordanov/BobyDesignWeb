import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Order } from 'src/app/models/order.model';

@Component({
  selector: 'app-print-order',
  templateUrl: './print-order.component.html',
  styleUrls: ['./print-order.component.scss']
})
export class PrintOrderComponent implements OnInit {

  
  constructor(private activatedRoute: ActivatedRoute, private router: Router,) { }
  order?: Order

  ngOnInit(): void {
    this.activatedRoute.data.subscribe((response: any) => {
      this.order = response.order;
      
      setTimeout(() => {
        const hideOnPrintElements = $('.hide-on-print');
        hideOnPrintElements.hide();
        window.print();
        hideOnPrintElements.show();
        this.router.navigateByUrl('orders/details?orderId='+this.order?.id);
      }, 1000);
    });
  }

  workMaterialsString(): string{
    return this.order?.craftingComponents.filter(cc => !cc.isDeposit).map(cc => cc.workMaterial.name).join(', ') || ''
  }

  getDepositComponentsString(): string {
    return this.order?.craftingComponents.filter(cc => cc.isDeposit)
      .map(cc=> `${cc.workMaterial.name}(${cc.quantity} ${cc.workMaterial.measuringUnit})`).join(', ') || '';
  }

  ngAfterViewInit()
  {
    
    //$(this.loginPartial.nativeElement).append(loginContent);
  }

}
