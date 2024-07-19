import { Component, input, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  // pierwszy sposob //
  //@Input() usersFromHomeComponent: any; //za pomocy dekorotywnego "input'u" i wlasciwosci "input'a", chcemy przekazac wlasciwosci ("users" w pliku "register.component.html") do komponentu podrzednego

  // drugi sposob - dziala z wersji 17.3^ //
  usersFromHomeComponent= input.required<any>()
  model: any = {}

  register() {
    console.log(this.model);
  }

  cancel() {
    console.log("cancelled")
  }
}
