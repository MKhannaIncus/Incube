import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Transaction } from "../_models/transactions";

@Injectable({
    providedIn: 'root'
})
export class TransactionService {
    private apiUrl = 'http://localhost:5000/api/transactions';

    constructor(private http: HttpClient) { }

    // Get all transactions based on the provided model (optional params)
    getTransactions(model: any): Observable<Transaction[]> {
        const params = new HttpParams({ fromObject: model });
        return this.http.get<Transaction[]>(this.apiUrl, { params });
    }

    // Get all transactions for a deal based on deal_Name (string)
    getDealsTransactions(deal_Name: string): Observable<Transaction[]> {
        // Fetch transactions for a specific deal using deal_Name
        return this.http.get<Transaction[]>(`${this.apiUrl}/transactionsfromdeal/${deal_Name}`);
    }

    // Add a new disbursement
    addDisbursement(model: any): Observable<Transaction> {
        return this.http.post<Transaction>(`${this.apiUrl}/NewTransactionDisbursement`, model);
    }

    // Add a new repayment
    addRepayment(model: any): Observable<Transaction> {
        return this.http.post<Transaction>(`${this.apiUrl}/NewTransactionRepayment`, model);
    }

    // Get projections for a deal based on deal_Name (string)
    getProjections(deal_Name: string): Observable<Transaction[]> {
        // Fetch projections for a specific deal using deal_Name
        return this.http.get<Transaction[]>(`${this.apiUrl}/Projections/${deal_Name}`);
    }
}
