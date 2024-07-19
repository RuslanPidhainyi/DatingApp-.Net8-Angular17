import { Component, EventEmitter, inject, input, Input, output, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  private accountService = inject(AccountService);

  // pierwszy sposob //
  //@Input() usersFromHomeComponent: any; //za pomocy dekorotywnego "input'u" i wlasciwosci "input'a", chcemy przekazac wlasciwosci ("users" w pliku "register.component.html") do komponentu podrzednego

  // drugi sposob - dziala z wersji 17.3^ //
  //usersFromHomeComponent= input.required<any>();

  /******************************************************************************************************** */

  // pierwszy sposob //
  //@Output() cancelRegister = new EventEmitter() //dekorator "@output()" czyli dekorator wyjsciowy - ktory bedzie cos robic i zaincjąwaliscmy EventEmitter - emiter zdarzen 

  // drugi sposob - dziala z wersji 17.3^ //
  cancelRegister = output<boolean>();

  model: any = {}

  register() {
    this.accountService.register(this.model).subscribe({
      next: response => {
        console.log(response);
        this.cancel();
      },
      error: error => console.log(error),
    })
  }

  cancel() {
    this.cancelRegister.emit(false); //kiedy klikamy na przycisk wewnetrz naszego komponentu podrzedniego emitujemy wartosc false 
  }
}
