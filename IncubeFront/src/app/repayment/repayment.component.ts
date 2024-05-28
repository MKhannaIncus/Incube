import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-repayment',
  templateUrl: './repayment.component.html',
  styleUrls: ['./repayment.component.css']
})
export class RepaymentComponent {
  repaymentForm: FormGroup;

  constructor(private formBuilder: FormBuilder) {
    this.repaymentForm = this.formBuilder.group({
      cashInterest: ['', Validators.required],
      pikInterest: ['', Validators.required],
      principal: ['', Validators.required],
      undrawnInterest: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.repaymentForm.valid) {
      console.log('Repayment Form Values:', this.repaymentForm.value);
      // Add logic to handle form submission, such as sending data to a service
      this.repaymentForm.reset();
    } else {
      console.error('Form is invalid');
    }
  }
}
