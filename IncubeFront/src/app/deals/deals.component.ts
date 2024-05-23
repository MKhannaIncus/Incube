import { Component, ViewChild } from '@angular/core';
import { DealService } from '../_services/deal.service';
import { Data } from '@angular/router';
import { Deal } from '../_models/deal';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';



@Component({
  selector: 'app-deals',
  templateUrl: './deals.component.html',
  styleUrls: ['./deals.component.css']
})
export class DealsComponent {
  model: any = {};
  deals : Deal[] = [];
  dealId!: number;

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
}
