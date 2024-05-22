import { Component, OnInit } from '@angular/core';
import { Transaction } from '../_models/transactions';
import { TransactionService } from '../_services/transactions.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})
export class TransactionsComponent {
  transactions : Transaction[] = [];
  model: any = {};


  constructor(private TransactionService: TransactionService,private router: Router){}
  
  ngOnInit(): void {
    this.transactionTable();
  }

  transactionTable(){
    this.TransactionService.getTransactions(this.model).subscribe(
      (data: Transaction[]) =>{
      this.transactions = data;
    });
  }
}
