import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
//import { NgIf } from '@angular/common'; /* Директива *ngIf зазвичай використовується в Angular-шаблонах для контролю видимості елементів. Якщо вираз всередині *ngIf оцінюється як true, елемент та його діти будуть включені в DOM; якщо оцінюється як false, елемент та його діти будуть видалені з DOM.              ALE derektywa angulara "NgIf" od wersji 17 angular nieuzywamy */

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule], //importujemy tutaj rozne dodatke do naszego projektu (np.FormsModule lub derektywe angulara "NgIf" )
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService);
  model: any = {};

  login(){
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
      },
      error: error => console.log(error),
    })
  }

  logout(){
    this.accountService.logout();
  }

}
