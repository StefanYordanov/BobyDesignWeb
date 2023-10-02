import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable, catchError, from, of } from "rxjs";
import { Injectable } from "@angular/core";
import { UserService } from "src/app/services/user.service";

@Injectable({
    providedIn: 'root'
})
export class AllUsersResolverService implements Resolve<any> {
    constructor(private userService: UserService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<any> {
        console.log('Called Get Product in resolver...', route);
        const page = Number(route.queryParamMap.get('page'));
        const role = route.queryParamMap.get('role') || undefined;
        const searchPhrase = route.queryParamMap.get('searchPhrase') || undefined;
        return from(this.userService.searchUsers(page, role, searchPhrase)).pipe(
            catchError(error => {
                return of('No data:' + error);
            })
        );
    }
}