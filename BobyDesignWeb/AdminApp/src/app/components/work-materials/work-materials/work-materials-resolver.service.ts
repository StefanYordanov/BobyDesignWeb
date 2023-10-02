import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable, catchError, from, of } from "rxjs";
import { Injectable } from "@angular/core";
import { UserService } from "src/app/services/user.service";
import { WorkMaterialsService } from "src/app/services/work-materials.service";

@Injectable({
    providedIn: 'root'
})
export class WorkMaterialsResolverService implements Resolve<any> {
    constructor(private workMaterialsService: WorkMaterialsService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<any> {
        console.log('Called work materials in resolver...', route);
        return from(this.workMaterialsService.getAllWorkMaterials()).pipe(
            catchError(error => {
                return of('No data:' + error);
            })
        );
    }
}