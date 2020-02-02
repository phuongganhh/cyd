import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }
  private Host: string = "http://localhost:50779/";
  public Get<T>(url: string) : Observable<T>{
    return this.http.get<T>(this.Host + url);
  }
}
