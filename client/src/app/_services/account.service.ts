import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';

//@Injectable - dekorator i mowi ze jest to wstrzykiwalne, co oznacza, ze jest to dekorator - ktory mowi ze mozemy uzyc tego komponentu lub servece i wstrzyknac ją do naszych komponentow  
@Injectable({
  providedIn: 'root'//jest on dostarczany w katalogu glownym naszej aplikacji i z uslugami 
})
export class AccountService {
  private http =  inject(HttpClient);

  baseUrl = 'https://localhost:5001/api/'
  currentUser = signal<User | null>(null)

  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(user => {
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);  
        }
      })
    )
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
