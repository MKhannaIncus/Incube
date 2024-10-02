import { Component, OnInit } from '@angular/core';
import { Transaction } from '../_models/transactions';
import { TransactionService } from '../_services/transactions.service';
import { ActivatedRoute } from '@angular/router';
import { DealService, FacilityInformation } from '../_services/deal.service';
import { Deal } from '../_models/deal';
import { FinancialMetrics } from '../_models/financialMetrics';
import { FinancialMetricsService } from '../_services/financial-metrics.service';

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})
export class TransactionsComponent implements OnInit {
  transactions: Transaction[] = [];
  projections: Transaction[] = [];
  dealInformation: Deal[] = [];
  public deal_Name!: string; // Changed from dealId to deal_Name
  model: any = {};
  popup = false;
  isRepayment: boolean = false;
  isDisbursement: boolean = false;
  facilityInformation: FacilityInformation | undefined;
  metrics: FinancialMetrics | undefined;

  constructor(
    private transactionService: TransactionService,
    private dealService: DealService,
    private financialMetricsService: FinancialMetricsService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const dealNameParam = params.get('deal_Name');  // Get deal_Name from the route parameters
      console.log('dealNameParam from URL:', dealNameParam);  // Debug log
  
      if (dealNameParam) {
        this.deal_Name = dealNameParam;  // Assign dealName to the class property
        this.transactionTable();         // Fetch transactions using dealName
        this.projectionsTable();         // Fetch projections using dealName
        this.fetchDealDetails(this.deal_Name);  // Fetch deal details using dealName
        this.facilityStatus(this.deal_Name);    // Fetch facility status using dealName
        // this.financialMetrics(this.deal_Name);  // Fetch financial metrics using dealName
      } else {
        console.error('dealNameParam is null or undefined');
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
  
  // Use deal_Name instead of dealId for transactions
  transactionTable() {
    this.transactionService.getDealsTransactions(this.deal_Name).subscribe((data: Transaction[]) => {
      this.transactions = data;
    }, error => {
      console.error('Error fetching transactions', error);
    });
  }

  // Use deal_Name instead of dealId for projections
  projectionsTable() {
    this.transactionService.getProjections(this.deal_Name).subscribe((data: Transaction[]) => {
      this.projections = data;
    }, error => {
      console.error('Error fetching projections', error);
    });
  }

  // Use deal_Name instead of dealId for fetching deal details
  fetchDealDetails(deal_Name: string) {
    this.dealService.dealInformation(deal_Name).subscribe(data => {
      console.log("Deal:", data);
      this.dealInformation = data;
    }, error => {
      console.error('Error fetching deal details', error);
    });
  }

  // Use deal_Name instead of dealId for facility status
  facilityStatus(deal_Name: string): void {
    this.dealService.facilityStatus(deal_Name).subscribe(data => {
      this.facilityInformation = data;
    }, error => {
      console.error('Error fetching facility status', error);
    });
  }

  // // Use deal_Name instead of dealId for financial metrics
  // financialMetrics(deal_Name: string): void {
  //   this.financialMetricsService.getFinancialMetricsByName(deal_Name).subscribe(data => {
  //     this.metrics = data;
  //   }, error => {
  //     console.error('Error fetching financial metrics', error);
  //   });
  // }
}
