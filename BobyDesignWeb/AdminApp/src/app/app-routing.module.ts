import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllUsersComponent } from './components/users/all-users/all-users.component';
import { AllUsersResolverService } from './components/users/all-users/all-users-resolver.service';
import { WorkMaterialsComponent } from './components/work-materials/work-materials/work-materials.component';
import { WorkMaterialsResolverService } from './components/work-materials/work-materials/work-materials-resolver.service';
import { CustomersComponent } from './components/customers/customers.component';
import { CustomersResolverService } from './components/customers/cusromers-resolver.service';
import { MainPageComponent } from './components/main-page/main-page.component';
import { OrdersViewComponent } from './components/orders/orders-view/orders-view.component';
import { OrdersViewResolverService } from './components/orders/orders-view/orders-view-resolver.service';
import { CreateOrderComponent } from './components/orders/create-order/create-order.component';
import { OrderDetailsComponent } from './components/orders/order-details/order-details.component';
import { OrderDetailsResolverService } from './components/orders/order-details/order-details-resolver.service';
import { PrintOrderComponent } from './components/orders/print-order/print-order.component';
import { EditOrderComponent } from './components/orders/edit-order/edit-order.component';
import { WorkMaterialsReportComponent } from './components/reports/work-materials-report/work-materials-report.component';

const routes: Routes = [
  {path: '', component: MainPageComponent },
  {path: 'administration/users', component: AllUsersComponent, resolve: {
    userPage: AllUsersResolverService
  }},
  { path: 'administration/work-materials-report', 
    component: WorkMaterialsReportComponent
  },
  {path: 'orders-view', component: OrdersViewComponent, resolve: {
    ordersPage: OrdersViewResolverService
  }},
  {path: 'orders/create', component: CreateOrderComponent},
  {path: 'orders/details', component: OrderDetailsComponent, resolve: {
    order: OrderDetailsResolverService
  }},
  {path: 'orders/print', component: PrintOrderComponent, resolve: {
    order: OrderDetailsResolverService
  }},
  {path: 'orders/edit', component: EditOrderComponent, resolve: {
    order: OrderDetailsResolverService
  }},

  {path: 'work-materials', component: WorkMaterialsComponent, resolve: {
    workMaterials: WorkMaterialsResolverService
  }},
  {path: 'customers', component: CustomersComponent, resolve: {
    customersPage: CustomersResolverService
  }},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
