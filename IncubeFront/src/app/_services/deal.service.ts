import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Deal } from '../_models/deal'; // Adjust path as necessary

@Injectable({
  providedIn: 'root'
})
export class DealService {
  private apiUrl = 'https://localhost:5001/api/deal';

  constructor(private http: HttpClient) { }

  getDeals(model: any): Observable<Deal[]> {
    const params = new HttpParams({ fromObject: model });
    return this.http.get<Deal[]>(this.apiUrl, { params });
  }


  createDeal(model: any) {
    return this.http.post<Deal>(this.apiUrl, model);
  }

}




