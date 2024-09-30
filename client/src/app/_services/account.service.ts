import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';
import { LikesService } from './likes.service';

//@Injectable - dekorator i mowi ze jest to wstrzykiwalne, co oznacza, ze jest to dekorator - ktory mowi ze mozemy uzyc tego komponentu lub servece i wstrzyknac ją do naszych komponentow
@Injectable({
  providedIn: 'root', //jest on dostarczany w katalogu glownym naszej aplikacji i z uslugami // Це робить сервіс доступним в усьому додатку.
})
export class AccountService {
  private http = inject(HttpClient); // Інжектуємо HttpClient для здійснення HTTP-запитів.
  private likeService = inject(LikesService);
  baseUrl = environment.apiUrl; // Встановлюємо базовий URL API з файлу конфігурації середовища.
  currentUser = signal<User | null>(null);
  roles = computed(() => {
    const user = this.currentUser();
    if (user && user.token) {
      const role = JSON.parse(atob(user.token.split('.')[1])).role
      return Array.isArray(role) ? role : [role];
    }
    return [];
  })

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      // post<User> - musimy zaznaczyć nasz obiekt User, zebys moglibysmy skorzystac z signala.(Bez tego nie da się)

      //return this.http.post<User>(this.baseUrl + 'account/login', model).pipe( - ", model" model po przeczynku oznacza to ze "Nastepnie wyslemy nazwe uzytkownika i haslo jako obiekt ktory bedzie czescią tresco HTTP"

      //map - operator "map" - ktory otzrymujemu z RxJS i to pomoge nam przekształcic lub zrobic cos z odpowiedzia zwrotną ktora otrzymamu z naszego API

      //map(user => - czyli user zwraca dane usera z naszego API
      map((user) => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user) => {
        if (user) {
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
    this.likeService.getLikeIds();
  }

  logout() {
    localStorage.removeItem('user'); //konwertujemy w zapis JSON i zapisujemy w localStorege
    this.currentUser.set(null); //ustawiamy sygnal na Userze
  }
}

/*
//currentUser = signal<User | null>(null);

  1) signal — це новий API в Angular (з версії 16), який дозволяє створювати реактивні змінні, що можуть автоматично відслідковувати зміни стану і автоматично оновлювати частини вашого додатка, що залежать від цих змін.

//signal<User | null>(null)

  - signal створює реактивну змінну з початковим значенням null. Це означає, що на початку, коли користувач ще не аутентифікований, змінна currentUser буде містити null

  - Коли користувач успішно аутентифікується, ми оновлюємо значення цього сигналу, викликаючи метод set на ньому (this.currentUser.set(user);), передаючи об'єкт користувача

  Висновок
currentUser = signal<User | null>(null); створює реактивну змінну для зберігання стану поточного користувача. Використовуючи signal, Angular може автоматично оновлювати частини вашого додатка, які залежать від цієї змінної, коли її значення змінюється. Це спрощує управління станом і покращує продуктивність вашого додатка.

 */

/*

// login(model: any) {
//   return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
//     map(user => {
//       if (user) {
//         localStorage.setItem('user', JSON.stringify(user)); // Конвертуємо користувача в JSON і зберігаємо в localStorage.
//         this.currentUser.set(user); // Встановлюємо сигнал поточного користувача.
//       }
//       return user;
//     })
//   );
// }


  1) HTTP-запит на вхід (POST запит): 
  
  //return this.http.post<User>(this.baseUrl + 'account/login', model)


    - this.http.post<User>: Виконує HTTP POST запит до сервера на маршрут 'account/login'
    - this.baseUrl + 'account/login': Конструює повний URL для запиту, використовуючи базову URL API (this.baseUrl) і шлях 'account/login'
    - model: Надсилає дані користувача (model) як тіло запиту.
    - <User>: Тип об'єкта, який очікується в відповіді (в цьому випадку — об'єкт типу User).


  2) Використання pipe для обробки відповіді:

  //  .pipe(
  //  map(user => {
  //    // ...
  //  })
  // )

  - .pipe: Метод RxJS, який використовується для застосування операторів до потоку даних. Тут використовується оператор map для обробки відповіді з сервера.

  3) Оператор map:

//   map(user => {
//   if (user) {
//     localStorage.setItem('user', JSON.stringify(user)); // Конвертуємо користувача в JSON і зберігаємо в localStorage.
//     this.currentUser.set(user); // Встановлюємо сигнал поточного користувача.
//   }
//   return user;
// })

  - map: Оператор RxJS, який застосовується до потоку даних. В даному випадку, він приймає user — об'єкт користувача, отриманий з відповіді сервера.
  - Якщо user існує (тобто сервер повернув валідні дані користувача після входу):

    - localStorage.setItem('user', JSON.stringify(user));
      - Зберігає об'єкт user в локальному сховищі браузера (localStorage).
      - Дані користувача конвертуються в JSON-рядок за допомогою JSON.stringify(user).
      - Це дозволяє зберегти сесію користувача між оновленнями сторінки або закриттям браузера.
  
    - this.currentUser.set(user);
      - Використовує сигнал (реактивну змінну) currentUser для збереження стану поточного користувача.
      - Оновлює стан додатка, щоб інші компоненти могли реагувати на зміну користувача
  
  4) Повернення результату

  // return user;

  - Після обробки даних оператор map повертає об'єкт user
  - Оскільки ми використовуємо pipe і map, весь метод login повертає Observable, який можна використовувати в підписках (subscribe) для подальшої обробки або в компонентах.


Підсумок
  - Запит на вхід: Метод login виконує POST запит до API для аутентифікації користувача.
  - Обробка відповіді: За допомогою оператора map метод обробляє відповідь з сервера. Якщо користувач успішно аутентифікований:
    - Зберігає його дані в localStorage для збереження сесії.
    - Оновлює сигнал currentUser, щоб інші частини додатка могли знати про зміну стану користувача.
  - Повернення Observable: Метод повертає Observable, який можна підписати для подальших дій у компонентах або сервісах Angular.

 */
