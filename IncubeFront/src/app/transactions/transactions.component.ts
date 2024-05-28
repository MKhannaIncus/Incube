import { Component, OnInit } from '@angular/core';
import { Transaction } from '../_models/transactions';
import { TransactionService } from '../_services/transactions.service';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})
export class TransactionsComponent {
  transactions : Transaction[] = [];
  public dealId!: number;
  model: any = {};
  popup =false;
  isRepayment: boolean = false;
  isDisbursement: boolean = false;


  constructor(private TransactionService: TransactionService, private route: ActivatedRoute){}
  
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const dealIdParam = params.get('dealId');
      console.log('dealIdParam from URL:', dealIdParam); // Debug log
      if (dealIdParam) {
        this.dealId = +dealIdParam;
        this.transactionTable();
      } else {
        console.error('dealIdParam is null or undefined');
      }
    });
  }

  togglePopup(type: string): void {
    this.popup = true;
    this.isRepayment = type === 'repayment';
    this.isDisbursement = type === 'disbursement';
  }

  closePopup(): void {
    this.popup = false;
    this.isRepayment = false;
    this.isDisbursement = false;
  }
  
  transactionTable(){
    this.TransactionService.getDealsTransactions(this.dealId).subscribe((data: Transaction[]) =>{
      this.transactions = data;
    });
  }


}
