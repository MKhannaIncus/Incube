import { Component, OnInit } from '@angular/core';
import { Transaction } from '../_models/transactions';
import { TransactionService } from '../_services/transactions.service';
import { ActivatedRoute } from '@angular/router';
import { DealService ,FacilityInformation } from '../_services/deal.service';
import { Deal } from '../_models/deal';
import { BackgroundTaskService } from '../_services/background-task.service'; // Import the BackgroundTaskService


@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})
export class TransactionsComponent {
  transactions : Transaction[] = [];
  dealInformation : Deal[] = [];
  public dealId!: number;
  model: any = {};
  popup =false;
  isRepayment: boolean = false;
  isDisbursement: boolean = false;
  facilityInformation: FacilityInformation | undefined ;



  constructor(private TransactionService: TransactionService, private DealService: DealService, private route: ActivatedRoute){}
  
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const dealIdParam = params.get('dealId');
      console.log('dealIdParam from URL:', dealIdParam); // Debug log
      if (dealIdParam) {
        this.dealId = +dealIdParam;
        this.transactionTable();
        this.fetchDealDetails(this.dealId);
        this.facilityStatus(this.dealId); // Added this line
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

  fetchDealDetails(dealId: number) {
    this.DealService.dealInformation(dealId).subscribe(data => {
      console.log("Deal:", this.dealInformation)
      this.dealInformation = data;
    },
    error => {
      console.error('Error', error);
    });
  }

  facilityStatus(dealId: number) : void{
    this.DealService.facilityStatus(dealId).subscribe(data => {
      this.facilityInformation = data;
    },
    error => {
      console.error('Error', error);
    });
  }

}
