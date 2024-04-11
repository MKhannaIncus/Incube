import { Component } from '@angular/core';
import { DealService } from '../_services/deal.service';
import { Data } from '@angular/router';
import { Deal } from '../_models/deal';

@Component({
  selector: 'app-deals',
  templateUrl: './deals.component.html',
  styleUrls: ['./deals.component.css']
})
export class DealsComponent {
  model: any = {};
  deals : Deal[] = [];

  constructor(private dealService: DealService){}

  ngOnInit(): void{
    this.deal();
  }

  // Method to handle click events
  // deal() {
  //   this.dealService.getDeals(this.model).subscribe((data: Deal[]) => {
  //     this.deals = data;
  //   });
    // or perform other actions related to the deal

    deal() {
      this.dealService.getDeals(this.model).subscribe((data: Deal[]) => {
        this.deals = data;
      });
  }
}
