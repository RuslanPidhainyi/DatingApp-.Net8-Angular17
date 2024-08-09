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
  currentUser = signal<User | null>(null)//wartości się zmienia Sygnaly mogą zawierać dowolne wartości, od prymitywów po zlozone struktury danych. //Ja mam wartosc null

  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(// post<User> - musimy zaznaczyć nasz obiekt User? zebys moglibysmy skorzystac z signala.(Bez tego nie da się)
      
      //map - operator "map" - ktory otzrymujemu z RxJS i to pomoge nam przekształcic lub zrobic cos z odpowiedzia zwrotną ktora otrzymamu z naszego API

      //map(user => - czyli user zwraca dane usera z naszego API
      map(user => {
        if(user){
          localStorage.setItem('user', JSON.stringify(user));//konwertujemy w zapis JSON i zapisujemy w localStorege
          this.currentUser.set(user);//ustawiamy sygnal na Userze
        }
      })
    )
  }

  register(model: any){
    //return this.http.post<User>(this.baseUrl + 'account/register', model).pipe( - ", model" model po przeczynku oznacza to ze "Nastepnie wyslemy nazwe uzytkownika i haslo jako obiekt ktory bedzie czescią tresco HTTP"
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);  
        }
        return user;
      })
    )
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
