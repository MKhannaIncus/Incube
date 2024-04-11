import { Component, OnInit } from '@angular/core';
import { AppComponent } from '../app.component';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent implements OnInit {
  model: any = {}
  loggedIn = false;

  //Service injectable
  constructor(public accountService: AccountService, private router:Router){}

  ngOnInit(): void{
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: response =>{
        console.log(response);
        this.loggedIn =true;
        this.router.navigate(['/deals']);
      },
      error: error => console.log(error)
    })
    }

    //console.log(this.model);

  logout(){
    this.accountService.logout();
  }

  // constructor(private accountService: AccountService){  
  // }

  // ngOnInit(): void{

  // }

  
  // login() {
  //   this.accountService.login(this.model).subscribe(response => {
  //     console.log(response);
  //     this.loggedIn =true;
  //   }, error => {
  //     console.log(error);
  //   })
  // }
  //#endregion

}
