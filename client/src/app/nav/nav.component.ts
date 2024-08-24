import { Component, Inject, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TitleCasePipe } from '@angular/common';
//import { NgIf } from '@angular/common'; /* Директива *ngIf зазвичай використовується в Angular-шаблонах для контролю видимості елементів. Якщо вираз всередині *ngIf оцінюється як true, елемент та його діти будуть включені в DOM; якщо оцінюється як false, елемент та його діти будуть видалені з DOM.              ALE derektywa angulara "NgIf" od wersji 17 angular nieuzywamy */

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule, RouterLink, RouterLinkActive, TitleCasePipe], //importujemy tutaj rozne dodatke do naszego projektu (np.FormsModule lub derektywe angulara "NgIf" )
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService);
  private router = inject(Router);
  private toaster = inject(ToastrService);
  model: any = {};

  login(){
    this.accountService.login(this.model).subscribe({
      next: _ => {
        this.router.navigateByUrl('/members')
      },
      error: error => this.toaster.error(error.error),
    })
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}
