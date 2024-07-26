export interface CustomerModel {
    id: number;
    name: string;
    email: string;
    phoneNumber: string;
    totalOrdersCost: number;
    totalPaidCost: number;
    reviews: CustomerReviewModel[]
}

export interface CustomerReviewModel{
    id: number;
    text: string;
    type: CustomerReviewType;
    createdOn: Date
    customerId: number;
}

export enum CustomerReviewType{
    Positive=1, Neutral = 2, Negative = 3
};