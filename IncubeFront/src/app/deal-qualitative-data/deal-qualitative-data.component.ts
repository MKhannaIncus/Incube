import { Component, OnInit } from '@angular/core';
import { Transaction } from '../_models/transactions';
import { TransactionService } from '../_services/transactions.service';
import { ActivatedRoute } from '@angular/router';
import { FinancialMetricsService } from '../_services/financial-metrics.service';
import { DealService, FacilityInformation } from '../_services/deal.service';
import { Deal } from '../_models/deal';
import { FinancialMetrics } from '../_models/financialMetrics';


@Component({
  selector: 'app-deal-qualitative-data',
  templateUrl: './deal-qualitative-data.component.html',
  styleUrls: ['./deal-qualitative-data.component.css']
})
export class DealQualitativeDataComponent implements OnInit {
  public deal_Name!: string;
  dealInformation: Deal[] = [];
  facilityInformation: FacilityInformation | undefined;
  metrics: FinancialMetrics | undefined;

  constructor(
    private transactionService: TransactionService,
    private financialMetricsService: FinancialMetricsService,
    private dealService: DealService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const dealNameParam = params.get('deal_Name');
      if (dealNameParam) {
        this.deal_Name = dealNameParam;
        this.fetchDealDetails(this.deal_Name);  // Call fetchDealDetails here
        this.fetchFinancialMetrics(this.deal_Name);  // Fetch metrics information
      } else {
        console.error('dealNameParam is null or undefined');
      }
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


  fetchFinancialMetrics(deal_Name: string): void {
    this.financialMetricsService.getFinancialMetrics(deal_Name).subscribe(data => {
      console.log("Metrics:", data);
      this.metrics = data;
    }, error => {
      console.error('Error fetching financial metrics', error);
    });
  }


}
