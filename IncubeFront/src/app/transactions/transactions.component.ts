import { Component, OnInit } from '@angular/core';
import { TransactionService } from '../_services/transactions.service';
import { ActivatedRoute } from '@angular/router';
import { FinancialMetrics } from '../_models/financialMetrics';
import { FinancialMetricsService } from '../_services/financial-metrics.service';
import { Deal } from '../_models/deal';
import { CommonModule } from '@angular/common';
import { Transaction } from '../_models/transactions';


@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})

export class TransactionsComponent implements OnInit {
  transactions: Transaction[] = [];
  projections: Transaction[] = [];
  accrued: Transaction[] = [];
  public deal_Name!: string; // Changed from dealId to deal_Name


  constructor(
    private transactionService: TransactionService,
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
        this.accruedTable();
        // this.financialMetrics(this.deal_Name);  // Fetch financial metrics using dealName
      } else {
        console.error('dealNameParam is null or undefined');
      }
    });
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

    // Use deal_Name instead of dealId for projections
    accruedTable() {
      this.transactionService.getAccrued(this.deal_Name).subscribe((data: Transaction[]) => {
        this.accrued = data;
      }, error => {
        console.error('Error fetching projections', error);
      });
    }


}



