import { keyframes } from '@angular/animations';
import { HttpClient, HttpErrorResponse, HttpParams,  } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, lastValueFrom, map } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface Params{
  [param: string]: string | number | boolean | readonly (string | number | boolean)[]
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private dateRegex = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)$/;

  apiDomain: string
  constructor(private httpClient: HttpClient, private toastr: ToastrService) { 
    this.apiDomain = environment.apiDomain
  }

  get<T>(endPoint: string, queryParams: Params = {}): Promise<T | null> {
    
    return lastValueFrom(this.httpClient.get<T>(`${this.apiDomain}${endPoint}`, {
      params: queryParams
    }).pipe(
      map(x => {
        this.convertDates(x);
        return x;
      })
    )).catch((err: HttpErrorResponse) => {
      this.toastr.error(err.message, err.error);
      return null;
    })
  }

  post<T>(endPoint: string, body: object, queryParams: Params = {}): Promise<T | null> {
    
    return lastValueFrom(this.httpClient.post<T>(`${this.apiDomain}${endPoint}`, body, {
      params: queryParams
    })).catch((err: HttpErrorResponse) => {
      this.toastr.error(err.message, err.error);
      return null;
    })
  }

  private convertDates(object: any) {
    if (!object || !(object instanceof Object)) {
      return;
    }

    if (object instanceof Array) {
      for (const item of object) {
        this.convertDates(item);
      }
    }

    for (const key of Object.keys(object)) {
      const value = object[key];

      if (value instanceof Array) {
        for (const item of value) {
          this.convertDates(item);
        }
      }

      if (value instanceof Object) {
        this.convertDates(value);
      }

      if (typeof value === 'string' && this.dateRegex.test(value)) {
        object[key] = new Date(value);
      }
    }
  }
}
