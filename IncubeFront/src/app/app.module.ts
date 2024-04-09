import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
<<<<<<< Updated upstream
import { LoginPageComponent } from './login-page/login-page.component';
import { FormsModule } from '@angular/forms';

//responsable for loading the webpage

@NgModule({ //decorator
  declarations: [ //new components
    AppComponent, LoginPageComponent
=======

@NgModule({
  declarations: [
    AppComponent
>>>>>>> Stashed changes
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
<<<<<<< Updated upstream
    BrowserAnimationsModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent] //boostrapping appcomponent
=======
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
>>>>>>> Stashed changes
})
export class AppModule { }