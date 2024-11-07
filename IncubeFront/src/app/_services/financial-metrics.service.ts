import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FinancialMetrics } from '../_models/financialMetrics';

@Injectable({
  providedIn: 'root'
})
export class FinancialMetricsService {

  private apiUrl = 'https://localhost:5001/api/deal';

  constructor(private http: HttpClient) { }

  getFinancialMetrics(deal_Name:string): Observable<FinancialMetrics>{
    return this.http.get<FinancialMetrics>(`${this.apiUrl}/FacilityInformation/${deal_Name}`);
  }

}
