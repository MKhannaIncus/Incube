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
  deals : Deal[] = [];
  dealId!: number;
  selectedFund: string = '';
  selectedValue: number = 0;


  popup = false;
  
  constructor(private dealService: DealService, private route: ActivatedRoute, private router: Router){}

  ngOnInit(): void{
    this.deal();

  }

  // Method to handle click events
  // deal() {
  //   this.dealService.getDeals(this.model).subscribe((data: Deal[]) => {
  //     this.deals = data;
  //   });
    // or perform other actions related to the deal
    pageRedirection(dealId:number): void {
      if (dealId !== undefined && dealId !== null) {
        console.log('redirectin with dealId:', dealId);
        this.router.navigate(['/deal-transaction', dealId]);
      } else {
        console.error('dealId is undefined or null:', dealId);
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
    if(this.selectedFund == "FundIII"){
      this.selectedValue =3;
    }else if(this.selectedFund == "FundIV"){
      this.selectedValue =4;
    }
    else{
      this.selectedValue = 0;
    }

    return this.selectedValue;
  }

  getStatusClass(status: string): string{
    switch (status) {
      case 'on-track':
        return 'on-track';
      case 'pending':
        return 'pending';
      case 'waiting-for':
        return 'waiting-for';
      default:
        return '';
    }
  }  
}
