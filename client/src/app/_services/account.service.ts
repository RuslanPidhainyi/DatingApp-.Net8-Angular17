import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

//@Injectable - dekorator i mowi ze jest to wstrzykiwalne, co oznacza, ze jest to dekorator - ktory mowi ze mozemy uzyc tego komponentu lub servece i wstrzyknac ją do naszych komponentow  
@Injectable({
  providedIn: 'root'//jest on dostarczany w katalogu glownym naszej aplikacji i z uslugami 
})
export class AccountService {
  private http =  inject(HttpClient);

  baseUrl = 'https://localhost:5001/api/'

  login(model: any){
    return this.http.post(this.baseUrl + 'account/login', model);
  }
}
