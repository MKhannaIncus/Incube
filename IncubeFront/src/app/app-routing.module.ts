import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DealsComponent } from './deals/deals.component';
import { LoginPageComponent } from './login-page/login-page.component'; 


const routes: Routes = [
  { path: 'deals', component:DealsComponent},
  { path: 'login', component: LoginPageComponent }, // Add this line for the login route
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Redirect the empty path to '/login' (or wherever you prefer)
  // { path: '', redirectTo: '/deals', pathMatch: 'full' }, // Redirect empty path to '/deals'
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
