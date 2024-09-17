//import { CommonModule, NgFor } from '@angular/common';//W Bibliotece Angulara 17 CommonModule - jest część NgFor. NgFor - iteruję nam liste (czyli cos podobnego jak foreach w C#). Jezeli bedziemy wykorzystac sam NgFor, to sensownie zostawic go, a nie CommonModule.
import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './_services/account.service';
import { HomeComponent } from "./home/home.component";
import { NgxSpinner, NgxSpinnerComponent } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent, HomeComponent, NgxSpinnerComponent],//imports: [RouterOutlet, NgFor], - NgFor - iteruję nam liste ale mozemy wykorzystac zwyklego for'a
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

//implements OnInit - Oczekuje od nas utworzenia func o nazwie ng on init, w ktorej moze wykonywac kod wymagania w tym konkretnym zdarzeniu cykl zycia 
export class AppComponent implements OnInit {
  //Kiedy chcemy uzyskac dostep do wlasciwosci class, musimy uzyc tego (czylic block kodu func ngOnInit):
  private accountService  = inject(AccountService);//To jest Wstrzykiwania zaleznosci

  //ng on Init - wykonuje nasze wywołanie HTTP
  ngOnInit(): void {
   this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if(!userString) return;//Jezeli nie mamy łancucha usera, nie mozemy z nim nic zrobic to wtedy wychodzi,y z tej metody

    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

  
}
