import { keyframes } from '@angular/animations';
import { HttpClient, HttpErrorResponse, HttpParams,  } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, lastValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface Params{
  [param: string]: string | number | boolean | readonly (string | number | boolean)[]
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {


  apiDomain: string
  constructor(private httpClient: HttpClient, private toastr: ToastrService) { 
    this.apiDomain = environment.apiDomain
  }

  get<T>(endPoint: string, queryParams: Params = {}): Promise<T | null> {
    
    return lastValueFrom(this.httpClient.get<T>(`${this.apiDomain}${endPoint}`, {
      params: queryParams
    })).catch((err: HttpErrorResponse) => {
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

}
