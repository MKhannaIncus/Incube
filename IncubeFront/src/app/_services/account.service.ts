import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';
import { User } from '../_models/user';
import { ReplaySubject } from 'rxjs';


//Services are injectable

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();


  constructor(private http: HttpClient) { }
 
  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: any) => { // Use any if the response does not directly match or use an appropriate interface
        const user: User = response as User; // Assert the type if confident about the structure
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(model: any){
    return this.http.post(this.baseUrl+'account/register', model).pipe(
      map((response: any) => { // Use any if the response does not directly match or use an appropriate interface
        const user: User = response as User; // Assert the type if confident about the structure
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }
  
  logout(){
   localStorage.removeItem('user');
   this.currentUserSource.next(null); 
  }
}

