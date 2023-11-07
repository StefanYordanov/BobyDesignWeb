import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable, catchError, from, of } from "rxjs";
import { Injectable } from "@angular/core";
import { UserService } from "src/app/services/user.service";
import { CustomersService } from "../../../services/customers.service";
import { OrdersService } from "src/app/services/orders.service";

@Injectable({
    providedIn: 'root'
})
export class OrdersViewResolverService implements Resolve<any> {
    constructor(private ordersService: OrdersService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<any> {
        console.log('Called Get Product in resolver...', route);
        const page = Number(route.queryParamMap.get('page'));
        const fromDate = route.queryParamMap.get('fromDate') || undefined;
        const toDate = route.queryParamMap.get('toDate') || undefined;
        const customerId = Number(route.queryParamMap.get('customerId')) || undefined;
        const searchPhrase = route.queryParamMap.get('searchPhrase') || undefined;
        return from(this.ordersService.getOrdersPagination({
            fromDate, toDate, customerId, searchPhrase
        }, page)).pipe(
            catchError(error => {
                return of('No data:' + error);
            })
        );
    }
}