import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginPageComponent } from './login-page/login-page.component';
import { FormsModule } from '@angular/forms';
import { DealsComponent } from './deals/deals.component';
import { AddNewDealComponent } from './add-new-deal/add-new-deal.component';
import { TransactionsComponent } from './transactions/transactions.component';

//responsable for loading the webpage

@NgModule({ //decorator
  declarations: [ //new components
    AppComponent, LoginPageComponent, DealsComponent, AddNewDealComponent, TransactionsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent] //boostrapping appcomponent
})
export class AppModule { }