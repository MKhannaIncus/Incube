import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
<<<<<<< Updated upstream
  templateUrl: './app.component.html', //what is to be loaded
  styleUrls: ['./app.component.css'] //corresponding css file
})
export class AppComponent implements OnInit {
  title = 'Incube';
=======
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating app';
>>>>>>> Stashed changes
  users: any;

  constructor(private http: HttpClient) {}

<<<<<<< Updated upstream
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
  
=======
  ngOnInit(): void {
    this.http.get('http://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log('Requst has completed')
    })
  }

>>>>>>> Stashed changes

}