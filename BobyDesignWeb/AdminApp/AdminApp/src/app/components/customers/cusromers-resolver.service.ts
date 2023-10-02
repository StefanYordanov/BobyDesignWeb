import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable, catchError, from, of } from "rxjs";
import { Injectable } from "@angular/core";
import { UserService } from "src/app/services/user.service";
import { CustomersService } from "../../services/customers.service";

@Injectable({
    providedIn: 'root'
})
export class CustomersResolverService implements Resolve<any> {
    constructor(private customersService: CustomersService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<any> {
        console.log('Called Get Product in resolver...', route);
        const page = Number(route.queryParamMap.get('page'));
        const searchPhrase = route.queryParamMap.get('searchPhrase') || undefined;
        return from(this.customersService.getCustomers(page, searchPhrase)).pipe(
            catchError(error => {
                return of('No data:' + error);
            })
        );
    }
}