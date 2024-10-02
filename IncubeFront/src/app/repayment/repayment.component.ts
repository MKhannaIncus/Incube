import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TransactionsComponent } from '../transactions/transactions.component';
import { TransactionService } from '../_services/transactions.service';
import { Transaction } from '../_models/transactions';


@Component({
  selector: 'app-repayment',
  templateUrl: './repayment.component.html',
  styleUrls: ['./repayment.component.css']
})
export class RepaymentComponent {
  repaymentForm: FormGroup;

  constructor(private formBuilder: FormBuilder, 
    public transactionsComponent: TransactionsComponent, 
    public transactionsService: TransactionService) {
    this.repaymentForm = this.formBuilder.group({
      repayment: ['', Validators.required],
      // cashInterest: ['', Validators],
      // pikInterest: ['', Validators],
      // principal: ['', Validators],
      // undrawnInterest: ['', Validators]
    });
  }

  onSubmit() {
    if (this.repaymentForm.valid) {
      console.log('Repayment Form Values:', this.repaymentForm.value);
      // Add logic to handle form submission, such as sending data to a service
      const repayment = this.repaymentForm.value.repayment;
      
      const transaction = {
        Transaction_Date : new Date(),
        Related_Deal_Id: this.transactionsComponent.deal_Name,
        Repayment: repayment
      };
      
      this.transactionsService.addRepayment(transaction).subscribe((response: Transaction)=> {
        console.log('Transaction response', response);
        this.repaymentForm.reset();
      },
      error => {
        console.error('Error', error);
      })
  }
  }
    // public onClick(id: number): void{
    //     this.modalService.close();
    //     //Your code goes here
    // }
}
