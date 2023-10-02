import { Component, OnInit } from '@angular/core';
import { PageView } from '../../models/common.model';
import { CustomerModel } from '../../models/customers.model';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomersService } from '../../services/customers.service';
import { Params } from '../../services/api.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.scss']
})
export class CustomersComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private router: Router,
    private customersService: CustomersService, private toastr: ToastrService) { }

  showNewCustomerMenu: boolean = false;
  newCustomer?: CustomerModel
  customersPage?: PageView<CustomerModel>;
  customerBeingEditedId?: number;
  customerBeingEdited?: CustomerModel;
  searchPhrase: string = '';

  ngOnInit(): void {
    this.activatedRoute.data.subscribe((response: any) => {
      this.customersPage = response.customersPage;
    });
    
    this.searchPhrase = this.activatedRoute.snapshot.queryParams["searchPhrase"];
  }

  async applySearch() {
    const queryParams: Params = { 
      searchPhrase: this.searchPhrase
  };
  this.router.navigate(
    [], 
    {
      relativeTo: this.activatedRoute,
      queryParams, 
      queryParamsHandling: 'merge', // remove to replace all query params by provided
    });
    this.customersPage = await this.customersService.getCustomers(0, this.searchPhrase) || undefined;
  }

  async switchPage(pageNumber: number) {
    const queryParams: Params = { 
      page: pageNumber
  };
  this.router.navigate(
    [], 
    {
      relativeTo: this.activatedRoute,
      queryParams, 
      queryParamsHandling: 'merge', // remove to replace all query params by provided
    });
    this.customersPage = await this.customersService.getCustomers(pageNumber, this.searchPhrase) || undefined;
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
    }
    this.toggleNewCustomerMenu();
    this.toastr.success("Успешно създаване на клиент")
  }

  toggleEditCustomer(customer: CustomerModel) {
    if (this.customerBeingEditedId === customer.id) {
      this.customerBeingEdited = undefined;
      this.customerBeingEditedId = undefined;
    } else {
      this.customerBeingEditedId = customer.id
      this.customerBeingEdited = {
        ...customer
      };
    }
  }

  async submitEditedEntry() {
    if (!this.customerBeingEdited) {
      return;
    }
    const editedEntry = await this.customersService.editCustomer(this.customerBeingEdited)
    this.customerBeingEdited = undefined;
    this.customerBeingEditedId = undefined;

    if(!this.customersPage || !editedEntry) {
      return
    }
    this.customersPage.items = this.customersPage.items.map(x => {
      if(x.id === editedEntry.id) {
        return editedEntry
      }
      return x;
    });

    this.toastr.success("Успешно редактиране на клиент")
  }

  cancelEditEntry() {
    this.customerBeingEdited = undefined;
    this.customerBeingEditedId = undefined;
  }
}
