import { Component } from '@angular/core';
import { AppComponent } from '../app.component';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  model: any = {}
  loggedIn = false;

  constructor(private accountService: AccountService){  
  }

  ngOnInit(): void{

  }

  login(){
    console.log(this.model);
    
    //create an observable
    this.accountService.login(this.model).subscribe({
      
    })
  }

}
