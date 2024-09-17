import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountService } from '../_services/account.service';

/*
Цей код використовується для автоматичного додавання токена автентифікації (JWT - JSON Web Token) до кожного HTTP-запиту, який надсилається з клієнтської сторони (Angular) до серверної (ASP.NET Web API). Таким чином, сервер отримує кожен запит вже з необхідним токеном у заголовку, що дозволяє ідентифікувати користувача і перевірити його права доступу.

Ваша реалізація jwtInterceptor забезпечує безпеку вашої програми, гарантуючи, що тільки аутентифіковані користувачі з дійсними токенами можуть отримати доступ до захищених ресурсів на сервері. 
*/

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);


  /*
  Перевіряється, чи є поточний користувач (currentUser()) у AccountService. Якщо так, то створюється клон запиту req з доданим заголовком Authorization, який містить токен (Bearer <токен>).
   */
  if (accountService.currentUser()) {
    req = req.clone({
      setHeaders:{
        Authorization: `Bearer ${accountService.currentUser()?.token}`
      }
    })
  }

  //Викликається next(req) для продовження обробки запиту.
  return next(req);
};
