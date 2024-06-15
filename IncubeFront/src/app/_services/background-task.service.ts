import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class BackgroundTaskService {
    private apiUrl = 'https://localhost:5001/backgroundtask';
  
    constructor(private http: HttpClient) {}
  
    getStatus(): Observable<any> {
      return this.http.get(`${this.apiUrl}/status`);
    }
  }