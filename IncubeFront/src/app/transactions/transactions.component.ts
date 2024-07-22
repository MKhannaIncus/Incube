import { Component, OnInit } from '@angular/core';
import { Transaction } from '../_models/transactions';
import { TransactionService } from '../_services/transactions.service';
import { ActivatedRoute } from '@angular/router';
import { DealService ,FacilityInformation } from '../_services/deal.service';
import { Deal } from '../_models/deal';
import { BackgroundTaskService } from '../_services/background-task.service'; // Import the BackgroundTaskService
import { FinancialMetrics } from '../_models/financialMetrics';
import { FinancialMetricsService } from '../_services/financial-metrics.service';

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})
export class TransactionsComponent {
  transactions : Transaction[] = [];
  projections : Transaction[]=[];
  dealInformation : Deal[] = [];
  public dealId!: number;
  model: any = {};
  popup =false;
  isRepayment: boolean = false;
  isDisbursement: boolean = false;
  facilityInformation: FacilityInformation | undefined ;
  metrics: FinancialMetrics | undefined;


  constructor(private TransactionService: TransactionService, private DealService: DealService, private FinancialMetrics: FinancialMetricsService, private route: ActivatedRoute){}
  
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const dealIdParam = params.get('dealId');
      console.log('dealIdParam from URL:', dealIdParam); // Debug log
      if (dealIdParam) {
        this.dealId = +dealIdParam;
        this.transactionTable();
        this.projectionsTable();
        this.fetchDealDetails(this.dealId);
        this.facilityStatus(this.dealId);
        this.financialMetrics(this.dealId);
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

  projectionsTable(){
    this.TransactionService.getProjections(this.dealId).subscribe((data: Transaction[]) =>{
      this.projections = data;
    });
    console.log("TEST");
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

  financialMetrics(dealId: number) : void{
    this.FinancialMetrics.getFinancialMetrics(dealId).subscribe(data => {
      this.metrics = data;
    },
    error => {
      console.error('Error', error);
    });
  }

}
