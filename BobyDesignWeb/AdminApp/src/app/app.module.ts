import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationMenuComponent } from './components/navigation-menu/navigation-menu.component';
import { HttpClientModule } from '@angular/common/http';
import { ApiService } from './services/api.service';
import { UserService } from './services/user.service';
import { AllUsersComponent } from './components/users/all-users/all-users.component';
import { CurrencyPipe, DatePipe, HashLocationStrategy, LocationStrategy } from '@angular/common';
import { AllUsersResolverService } from './components/users/all-users/all-users-resolver.service';
import { UserRightsComponent } from './components/users/user-rights/user-rights.component';
import { FormsModule } from '@angular/forms';
import { PaginationComponent } from './components/pagination/pagination.component';
import { WorkMaterialsComponent } from './components/work-materials/work-materials/work-materials.component';
import { WorkMaterialsService } from './services/work-materials.service';
import { WorkMaterialsResolverService } from './components/work-materials/work-materials/work-materials-resolver.service';
import { CustomersComponent } from './components/customers/customers.component';
import { CustomersService } from './services/customers.service';
import { CustomersResolverService } from './components/customers/cusromers-resolver.service';
import { OrdersCalendarComponent } from './orders-calendar/orders-calendar.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationMenuComponent,
    AllUsersComponent,
    UserRightsComponent,
    PaginationComponent, 
    WorkMaterialsComponent, CustomersComponent, OrdersCalendarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ToastrModule.forRoot()
  ],
  providers: [{provide: LocationStrategy, useClass:HashLocationStrategy}, 
    // {
    //   provide: LOCALE_ID,
    //   useValue: 'bg-BG' // 'de-DE' for Germany, 'fr-FR' for France ...
    // },
    DatePipe,
    CurrencyPipe,
    ApiService, 
    UserService, AllUsersResolverService,
    WorkMaterialsService, WorkMaterialsResolverService,
    CustomersService, CustomersResolverService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
