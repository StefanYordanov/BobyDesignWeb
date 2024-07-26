import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CustomerModel, CustomerReviewModel, CustomerReviewType } from 'src/app/models/customers.model';
import { CustomersService } from 'src/app/services/customers.service';

@Component({
  selector: 'app-customer-reviews-list',
  templateUrl: './customer-reviews-list.component.html',
  styleUrls: ['./customer-reviews-list.component.scss']
})
export class CustomerReviewsListComponent implements OnInit {

  showNewReviewForm = false;
  customerReviewType = CustomerReviewType;
  newReviewType = CustomerReviewType.Neutral;
  newReviewText = '';

  constructor(private customerService: CustomersService, private toastr: ToastrService) { }
  @Input() customer: CustomerModel
  ngOnInit(): void {
  }

  async submitNewReview(){
    console.log(CustomerReviewType.Negative, CustomerReviewType.Neutral, CustomerReviewType.Positive, this.newReviewType)
    if([CustomerReviewType.Negative, CustomerReviewType.Neutral, CustomerReviewType.Positive].indexOf(this.newReviewType) < 0){
      this.toastr.error('Невалиден тип оценка')
      return;
    }

    if(!this.customer || !this.customer.id){
      this.toastr.error('Не е зададен клиент')
      return;
    }

    const newCustomerReview: CustomerReviewModel = {
      id: 0,
      text: this.newReviewText,
      type: this.newReviewType,
      createdOn: new Date(),
      customerId: this.customer.id
    }

    const response = await this.customerService.addNewCustomerReview(newCustomerReview)

    if(response){
      this.customer.reviews.splice(0, 0, response);
      this.newReviewText = '';
      this.newReviewType = CustomerReviewType.Neutral
    }

    this.showNewReviewForm = false;
  }

}
