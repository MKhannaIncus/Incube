import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TransactionService } from '../_services/transactions.service';
import { Transaction } from '../_models/transactions';
import { TransactionsComponent } from '../transactions/transactions.component';

@Component({
  selector: 'app-disbursement',
  templateUrl: './disbursement.component.html',
  styleUrls: ['./disbursement.component.css']
})
export class DisbursementComponent {
  disbursementForm: FormGroup;

  constructor(private formBuilder: FormBuilder, public transactionService: TransactionService, public transactionsComponent: TransactionsComponent){
    this.disbursementForm= this.formBuilder.group({
      disbursement: ['', Validators.required]
    })
  }

  onSubmit(){
    if(this.disbursementForm.valid){
      const disbursementValue = this.disbursementForm.value.disbursement;

      const transaction = {
        Drawdown: disbursementValue,
        Transaction_Date : new Date(),
        Related_Deal_Id: this.transactionsComponent.dealId 
      };

      this.transactionService.addDisbursement(transaction).subscribe((response: Transaction) =>{
        console.log('Transaction response', response);
        this.disbursementForm.reset();
      },
      error => {
        console.error('Error', error);
      })

    }
  }

}
