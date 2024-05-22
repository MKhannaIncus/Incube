import { Component, ViewChild } from '@angular/core';
import { DealService } from '../_services/deal.service';
import { Data } from '@angular/router';
import { Deal } from '../_models/deal';
import { Router } from '@angular/router';


@Component({
  selector: 'app-deals',
  templateUrl: './deals.component.html',
  styleUrls: ['./deals.component.css']
})
export class DealsComponent {
  model: any = {};
  deals : Deal[] = [];
  
  popup = false;
  
  constructor(private dealService: DealService, private router: Router){}

  ngOnInit(): void{
    this.deal();
  }

  // Method to handle click events
  // deal() {
  //   this.dealService.getDeals(this.model).subscribe((data: Deal[]) => {
  //     this.deals = data;
  //   });
    // or perform other actions related to the deal
    pageRedirection(): void {
      this.router.navigate(['/deal-transaction']);
    }

    deal() {
      this.dealService.getDeals(this.model).subscribe((data: Deal[]) => {
        this.deals = data;
      });
  }
}
