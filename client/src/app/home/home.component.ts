import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  http = inject(HttpClient);
  registerMode = false; 
  users: any;

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle() {
    this.registerMode = !this.registerMode
  }

  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }

  getUsers(){
    /*
     Co robi nasz Kod, a dokladnie metoda get?
     
     Konstruuje więc żądanie Get, które interpretuje treść jako JSON i zwraca treść odpowiedzi jako obiekt parsowany z JSON

     A jesli spojrzymy na zwrot, to zwraca on coś, co nazywa się obserwowalnym adresem URL treści odpowiedzi jako objekt JSON - Teraz Angular intensywnie korzysta z tych rzeczy zwanych OBSERVABLES.

     pomysl o OBSERVABLES jako o strumieniu danych i mozemy obserwowac ten strumien, w tym przypadku obserwujemy odpowiedz HTTP powracającą z naszego serwera API 

     Typowym sposobem radzenia sobie z kodem asynchronicznym TS jest uzycie Promise lub asynchroniznych await, podobnie jak w C# Ale Angular jest inny. Wykorzystuje on obserwowalny.

     Jak wchodzimy w interakcje z obserwowalnym a 1 Zasada obserwowalnych, jesli chcesz z tym ządaniem HTTP jest takie, ze nic się nie stanie, jesli zostawiamy nasz kod w ten sposob, z tym żądaniem HTTP jest takie, że nic się nie stanie, jesli zostawimy nasz kod w ten sposób. Nie wykonujemy żądania HTTP.
   */

   // W ten sposob uzyskujemy dostęp do wlasciwosci klasy
   this.http.get('https://localhost:5001/api/users').subscribe({
     //func //otrzymujemy respons czyli odpowiedz z nazego API
     next: response => this.users = response,

     //Co biedzie jezeli wystąpi błąd
     error: error => console.log(error),

     //Co ma się wydarzyć po skonczeniu działania obserwowalnego - i znow jest to kolejna func, ktorą mozemy dodać, aby zrobić coś gdy coś się zakonczy
     complete: () => console.log('Request has completed')

   }) //.http.get - poniewarz zamierzymy wykonac żądanie get. Następnie okreslamy adress URL, skąd bedziemy pobierac dane - umorzliwia urzytkownikam pobieranie dane

   /*
     Obserwowalne są domyslnie leniwy i jesli nikt nie subskrebuje obserwalnego, nie robi on nic.

     Więc jesli chcemy, aby to się wykonało, musimy najpierw zasubskrybowac obserwowalne a potem mozemy powieszic co się stanie po subskrypcji
   */
 }
}
