import { Component, input, ViewEncapsulation } from '@angular/core';
import { Member } from '../../_models/member';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
  //encapsulation: ViewEncapsulation.None //enkapsulaja naszych componentów w angularze - jest domyslana. Kazdy komponent ma swoj unikalny identyfikator, a CSS zostanie zawarty w tym componencie
})
export class MemberCardComponent {
  member = input.required<Member>();//input jest roszezenie Signala. Czyli musimy uzy jako sygnal
}
