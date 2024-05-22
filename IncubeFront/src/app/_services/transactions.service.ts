import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Transaction } from "../_models/transactions";


@Injectable({
    providedIn: 'root'
})
export class TransactionService{
    private apiUrl = 'https://localhost:5001/api/transactions';

    constructor( private http: HttpClient) { }

    getTransactions(model: any): Observable<Transaction[]>{
        const params = new HttpParams({fromObject: model});
        return this.http.get<Transaction[]>(this.apiUrl,  { params });
    }


    // getTransactions(relatedDealId?: number): Observable<Transaction[]>{
    //     let params = new HttpParams();

    //     if(relatedDealId){
    //         params = params.append('relatedDealId', relatedDealId.toString());
    //     }
        
    //     return this.http.get<Transaction[]>(this.apiUrl);
    // }

}