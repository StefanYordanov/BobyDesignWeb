import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllUsersComponent } from './components/users/all-users/all-users.component';
import { AllUsersResolverService } from './components/users/all-users/all-users-resolver.service';
import { WorkMaterialsComponent } from './components/work-materials/work-materials/work-materials.component';
import { WorkMaterialsResolverService } from './components/work-materials/work-materials/work-materials-resolver.service';
import { CustomersComponent } from './components/customers/customers.component';
import { CustomersResolverService } from './components/customers/cusromers-resolver.service';

const routes: Routes = [
  {path: 'administration/users', component: AllUsersComponent, resolve: {
    userPage: AllUsersResolverService
  }},
  {path: 'work-materials', component: WorkMaterialsComponent, resolve: {
    workMaterials: WorkMaterialsResolverService
  }},
  {path: 'customers', component: CustomersComponent, resolve: {
    customersPage: CustomersResolverService
  }}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
