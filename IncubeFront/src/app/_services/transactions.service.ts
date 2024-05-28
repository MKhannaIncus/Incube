import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Transaction } from "../_models/transactions";


@Injectable({
    providedIn: 'root'
})
export class TransactionService{
    private apiUrl = 'http://localhost:5000/api/transactions';

    constructor( private http: HttpClient) { }

    getTransactions(model: any): Observable<Transaction[]>{
        const params = new HttpParams({fromObject: model});
        return this.http.get<Transaction[]>(this.apiUrl,  { params });
    }

    getDealsTransactions(dealId:number): Observable<Transaction[]>{
        const params = new HttpParams().set('dealId', dealId.toString());
        return this.http.get<Transaction[]>(`${this.apiUrl}/transactionsfromdeal/${dealId}`);
    }

    /*A new disbursement is added by the User*/ 
    addDisbursement(model:any): Observable<Transaction>{
        return this.http.post<Transaction>(`${this.apiUrl}/NewTransactionDisbursement`, model);
    }

    // /*A new repayment is added by the User*/
    // addRepayment(model:any): Observable<Transaction>{
    //     return this.http.
    // }




    // getTransactions(relatedDealId?: number): Observable<Transaction[]>{
    //     let params = new HttpParams();

    //     if(relatedDealId){
    //         params = params.append('relatedDealId', relatedDealId.toString());
    //     }
        
    //     return this.http.get<Transaction[]>(this.apiUrl);
    // }

}