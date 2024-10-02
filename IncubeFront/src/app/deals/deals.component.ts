import { Component, ViewChild, ViewEncapsulation } from '@angular/core';
import { DealService } from '../_services/deal.service';
import { Data } from '@angular/router';
import { Deal } from '../_models/deal';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { error } from 'jquery';


@Component({
  selector: 'app-deals',
  templateUrl: './deals.component.html',
  styleUrls: ['./deals.component.css'],
})
export class DealsComponent {
  model: any = {};
  deals: Deal[] = [];
  dealId!: number;
  selectedFund: string = '';
  selectedValue: number = 0;


  popup = false;

  constructor(private dealService: DealService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.deal();

  }
  // Method to handle click events
  // deal() {
  //   this.dealService.getDeals(this.model).subscribe((data: Deal[]) => {
  //     this.deals = data;
  //   });
  // or perform other actions related to the deal
  // Method to redirect to the deal transaction page using deal_Name
  pageRedirection(deal_Name: string): void {
    if (deal_Name) {
      console.log('Redirecting with deal_Name:', deal_Name);
      this.router.navigate(['/deal-transaction', deal_Name]); // Navigate using deal_Name
    } else {
      console.error('deal_Name is undefined or empty:', deal_Name);
    }
  }


  deal() {
    this.dealService.getDeals(this.model).subscribe((data: Deal[]) => {
      this.deals = data;
    });
  }

  onFundChange(): void {
    this.selectedValue = this.mapSelectedValue(this.selectedFund);
    if (this.selectedFund) {
      this.dealService.fundDeal(this.selectedValue).subscribe((data: Deal[]) => {
        this.deals = data;
      })
      console.log(`You have selected: ${this.selectedFund}`);
    } else {
      console.error(error);
    }
  }

  mapSelectedValue(selectedFund: string): number {
    if (this.selectedFund == "FundIII") {
      this.selectedValue = 3;
    } else if (this.selectedFund == "FundIV") {
      this.selectedValue = 4;
    }
    else {
      this.selectedValue = 0;
    }

    return this.selectedValue;
  }

  getStatusClass(status: string): string {
    let className = '';
    switch (status) {
      case 'On track':
        className = 'On-track';
        break;
      case 'Coming up':
        className = 'default';
        break;
      case 'Pending':
        className = 'pending';
        break;
      // Add more cases as needed
      default:
        className = 'default-class';
        break;
    }
    console.log(`Status: ${status}, Class: ${className}`);
    return className;
  }

getIconForSector(sector: string): string {
  switch (sector) {
    case 'Consumer Staples':
      return 'assets/consumer-staples.svg';
    case 'RE Hospitality':
      return 'assets/hospitality.svg';
    case 'RE Offices':
      return 'assets/offices.svg';
    case 'Materials':
      return 'assets/material.svg';
    case 'Energy':
      return 'assets/green-energy.svg';
    case 'RE Residential':
      return 'assets/residential.svg';
    case 'RE Retail':
      return 'assets/retail.svg';
    case 'Industrials':
      return 'assets/industrial.svg';
    case 'Transport':
      return 'assets/transport.svg';
    case 'Utilities':
      return 'assets/utilities.svg';
    default:
      return 'assets/default-icon.svg';
  }
}

}
