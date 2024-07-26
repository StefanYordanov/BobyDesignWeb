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
import { MainPageComponent } from './components/main-page/main-page.component';
import { CalendarViewComponent } from './components/calendar-view/calendar-view.component';
import { CalendarService } from './services/calendar.service';
import { OrdersService } from './services/orders.service';
import { ModalFrameComponent } from './components/modal-frame/modal-frame.component';
import { OrdersViewResolverService } from './components/orders/orders-view/orders-view-resolver.service';
import { OrdersViewComponent } from './components/orders/orders-view/orders-view.component';
import { CustomerPickerComponent } from './components/customers/customer-picker/customer-picker.component';
import { WorkMaterialPickerComponent } from './components/work-materials/work-material-picker/work-material-picker.component';
import { CreateOrderComponent } from './components/orders/create-order/create-order.component';
import { PriceCalculatorService } from './services/price-calculator.service';
import { DrawingCanvasComponent } from './drawing-canvas/drawing-canvas.component';
import { WebContentService } from './services/web-content.service';
import { BlobService } from './services/blob.service';
import { OrderDetailsComponent } from './components/orders/order-details/order-details.component';
import { PrintOrderComponent } from './components/orders/print-order/print-order.component';
import { TruncatePipe } from './services/truncate.pipe';
import { EditOrderComponent } from './components/orders/edit-order/edit-order.component';
import { DateService } from './services/date.service';
import { WorkMaterialsReportComponent } from './components/reports/work-materials-report/work-materials-report.component';
import { ReportsService } from './services/reports.service';
import { IconComponent } from './components/icon/icon.component';
import { SuppliersService } from './services/suppliers.service';
import { WorkMaterialDropdownSelectorComponent } from './components/work-materials/work-material-dropdown-selector/work-material-dropdown-selector.component';
import { CustomerReviewIconComponent } from './components/customers/customer-review-icon/customer-review-icon.component';
import { CustomerReviewsListComponent } from './components/customers/customer-reviews-list/customer-reviews-list.component';

@NgModule({
  declarations: [
    TruncatePipe,
    AppComponent,
    NavigationMenuComponent,
    AllUsersComponent,
    UserRightsComponent,
    PaginationComponent, 
    ModalFrameComponent,
    WorkMaterialsComponent, 
    CustomersComponent, 
    MainPageComponent, 
    CalendarViewComponent, 
    OrdersViewComponent, 
    CustomerPickerComponent, 
    WorkMaterialPickerComponent, 
    CreateOrderComponent, 
    DrawingCanvasComponent, 
    OrderDetailsComponent, 
    PrintOrderComponent, 
    EditOrderComponent,
    WorkMaterialsReportComponent,
    IconComponent,
    WorkMaterialDropdownSelectorComponent,
    CustomerReviewIconComponent,
    CustomerReviewsListComponent
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
    CalendarService, 
    OrdersService, OrdersViewResolverService,
    UserService, AllUsersResolverService,
    WorkMaterialsService, WorkMaterialsResolverService,
    CustomersService, CustomersResolverService,
    SuppliersService,
    PriceCalculatorService,
    WebContentService, BlobService, DateService, ReportsService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
