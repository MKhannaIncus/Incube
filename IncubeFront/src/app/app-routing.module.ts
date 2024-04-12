import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DealsComponent } from './deals/deals.component';
import { LoginPageComponent } from './login-page/login-page.component'; 
import { AddNewDealComponent } from './add-new-deal/add-new-deal.component';


const routes: Routes = [
  { path: 'deals', component:DealsComponent},
  { path: 'login', component: LoginPageComponent }, 
  { path: 'new-deal', component: AddNewDealComponent }, 
  { path: '', redirectTo: '/login', pathMatch: 'full' }, 
  // { path: '', redirectTo: '/deals', pathMatch: 'full' }, // Redirect empty path to '/deals'
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
