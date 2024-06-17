import { Component } from '@angular/core';
import { DealService } from '../_services/deal.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';


@Component({
  selector: 'app-add-new-deal',
  templateUrl: './add-new-deal.component.html',
  styleUrls: ['./add-new-deal.component.css']
})
export class AddNewDealComponent {
  dealForm: FormGroup;
  isPopupVisible = false;

  constructor(public dealService: DealService, private formBuilder: FormBuilder) {
    this.dealForm = this.formBuilder.group({
      deal_Id: [0], // Assuming 0 as default for new deals
      deal_Name: ['', Validators.required],
      client_Id: [null],
      facility: [null],
      asset_Id: [null],
      country: [null],
      sector: [null],
      subsector: [null],
      investment_date: [null],
      maturity_date: [null],
      opening_fee: [null],
      minimum_multiple: [null],
      underwriting_IRR: [null],
      underwriting_MOIC: [null],
      underwriting_NAV: [null],
      availability_period: [null],
      availability_fee: [null],
      intercompany_loan: [false],
      entity_Id: [null],
      interest_Id: [null],
      amortization_type: [null],
      ownership_Id: [null],
      LTV_Entry: [null]
    });
  }


  onSubmit() {
      this.dealService.createDeal(this.dealForm.value).subscribe({
        next: (response) => {
          console.log('Deal created successfully', response);
          // Optionally reset the form or redirect the user
          this.dealForm.reset();
          // this.router.navigate(['/deals']); // if you want to redirect
        },
        error: (error) => {
          console.error('Error creating deal:', error);
          // Handle errors, possibly show user feedback
        }
      })
      if (this.dealForm.valid) {
        // Handle form submission
        this.isPopupVisible = false;
      }
  }

  openPopup() {
    this.isPopupVisible = true;
  }

  closePopup() {
    this.isPopupVisible = false;
  }
  

}
