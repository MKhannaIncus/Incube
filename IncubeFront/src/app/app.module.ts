import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginPageComponent } from './login-page/login-page.component';
import { FormsModule } from '@angular/forms';
import { DealsComponent } from './deals/deals.component';
import { AddNewDealComponent } from './add-new-deal/add-new-deal.component';
import { TransactionsComponent } from './transactions/transactions.component';
import { DisbursementComponent } from './disbursement/disbursement.component';
import { RepaymentComponent } from './repayment/repayment.component';
import { DealDataComponent } from './deal-data/deal-data.component';

//responsable for loading the webpage

@NgModule({ //decorator
  declarations: [ //new components
    AppComponent, LoginPageComponent, DealsComponent, AddNewDealComponent, TransactionsComponent, DisbursementComponent, RepaymentComponent, DealDataComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    MatSelectModule,
    MatFormFieldModule
  ],
  providers: [],
  bootstrap: [AppComponent] //boostrapping appcomponent
})
export class AppModule { }