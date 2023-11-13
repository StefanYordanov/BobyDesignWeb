import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PageView } from 'src/app/models/common.model';
import { CustomerModel } from 'src/app/models/customers.model';
import { CustomersService } from 'src/app/services/customers.service';

@Component({
  selector: 'app-customer-picker',
  templateUrl: './customer-picker.component.html',
  styleUrls: ['./customer-picker.component.scss']
})
export class CustomerPickerComponent implements OnInit {

  constructor(
    private customersService: CustomersService, private toastr: ToastrService) { }

    @Output() customerChanged = new EventEmitter<CustomerModel>();
    
  showNewCustomerMenu: boolean = false;
  newCustomer?: CustomerModel
  customersPage?: PageView<CustomerModel>;
  selectedCustomer?: CustomerModel;
  searchPhrase: string = '';

  async ngOnInit()  {
    this.searchPhrase = '';
    this.customersPage = await this.customersService.getCustomers(0, this.searchPhrase) || undefined;
  }

  async applySearch() {
    this.customersPage = 
    await this.customersService.getCustomers(this.customersPage?.currentPage || 0, this.searchPhrase) || undefined;
  }

  async switchPage(pageNumber: number) {
    this.customersPage = await this.customersService.getCustomers(pageNumber, this.searchPhrase) || undefined;
  }

  toggleCustomer(customer:CustomerModel) {
    if(this.selectedCustomer && this.selectedCustomer.id === customer.id) {
      this.selectedCustomer = undefined;
    } else {
      
    this.selectedCustomer = customer;
    }
    this.customerChanged.emit(this.selectedCustomer);
  }

  toggleNewCustomerMenu() {
    this.showNewCustomerMenu = !this.showNewCustomerMenu;
    if (this.showNewCustomerMenu) {
      this.newCustomer = {
        id: 0,
        name: '',
        email: '',
        phoneNumber: '',
        totalOrdersCost: 0,
        totalPaidCost: 0
      }
    } else {
      this.newCustomer = undefined;
    }
  }

  async submitNewEntry() {
    if (!this.newCustomer) {
      return;
    }

    const model = await this.customersService.addNewCustomer(this.newCustomer);
    if (model && this.customersPage) {
      this.customersPage.items.push(model);
      this.selectedCustomer = model;
    }
    this.toggleNewCustomerMenu();
    this.toastr.success("Успешно създаване на клиент")
  }
}
