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

  constructor(public dealService: DealService, private formBuilder: FormBuilder) {
    this.dealForm = this.formBuilder.group({
      alias: ['', Validators.required]
    }
    );
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
  }

}
