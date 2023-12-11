import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable, catchError, from, of } from "rxjs";
import { Injectable } from "@angular/core";
import { OrdersService } from "src/app/services/orders.service";

@Injectable({
    providedIn: 'root'
})
export class OrderDetailsResolverService implements Resolve<any> {
    constructor(private ordersService: OrdersService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<any> {
        console.log('Called Get Product in resolver...', route);
        const orderId = Number(route.queryParamMap.get('orderId'));
        return from(this.ordersService.getOrder(orderId)).pipe(
            catchError(error => {
                return of('No data:' + error);
            })
        );
    }
}