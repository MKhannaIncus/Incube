import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html', //what is to be loaded
  styleUrls: ['./app.component.css'] //corresponding css file
})
export class AppComponent implements OnInit {
  title = 'Incube';
  users: any;
  showNavbar = true;


  constructor(private http: HttpClient, private router: Router) {
        // Listen to route changes
        this.router.events.subscribe((event) => {
          if (event instanceof NavigationEnd) {
            this.showNavbar = !this.router.url.includes('/login');
          }
        });
  }

  ngOnInit() {
    this.getUsers();
  }

  getUsers() {
    this.http.get('https://localhost:5001/api/users').subscribe(response => {
      this.users = response;
    }, error => {
      console.log(error);
    })
  }
}