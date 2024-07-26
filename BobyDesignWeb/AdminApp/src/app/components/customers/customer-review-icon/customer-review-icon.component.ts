import { Component, Input, OnInit } from '@angular/core';
import { CustomerReviewType } from 'src/app/models/customers.model';

@Component({
  selector: 'app-customer-review-icon',
  templateUrl: './customer-review-icon.component.html',
  styleUrls: ['./customer-review-icon.component.scss']
})
export class CustomerReviewIconComponent implements OnInit {

  //face-meh
  //face-smile-beam
  //face-frown
  constructor() { }
  @Input() type?: CustomerReviewType;
  customerReviewType = CustomerReviewType;
  ngOnInit(): void {
  }

  iconProps(type?: CustomerReviewType){
    if(!type) {
      return undefined;
    }

    switch(type){
      case CustomerReviewType.Positive: return { iconName: 'face-smile-beam', textClass: 'text-success' };
      case CustomerReviewType.Neutral: return { iconName: 'face-meh', textClass: 'text-secondary' };
      case CustomerReviewType.Negative: return { iconName: 'face-frown', textClass: 'text-danger' };
      default: return undefined;
    }
  }

}
