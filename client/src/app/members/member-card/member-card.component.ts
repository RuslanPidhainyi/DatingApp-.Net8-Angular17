import { Component, computed, inject, input, ViewEncapsulation } from '@angular/core';
import { Member } from '../../_models/member';
import { RouterLink } from '@angular/router';
import { LikesService } from '../../_services/likes.service';
import { PresenceService } from '../../_services/presence.service';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
  //encapsulation: ViewEncapsulation.None //enkapsulaja naszych componentów w angularze - jest domyslana. Kazdy komponent ma swoj unikalny identyfikator, a CSS zostanie zawarty w tym componencie
})
export class MemberCardComponent {
  private likeService = inject(LikesService);
  private presenceService = inject(PresenceService);  
  member = input.required<Member>();//input jest roszezenie Signala. Czyli musimy uzy jako sygnal
  hasLiked = computed(() => this.likeService.likeIds().includes(this.member().id))

  isOnline = computed(() => this.presenceService.onlineUsers().includes(this.member().username));

  toggleLike() {
    this.likeService.toggleLike(this.member().id).subscribe({
      next: () => {
        if(this.hasLiked()) {
          this.likeService.likeIds.update(ids => ids.filter(x => x !== this.member().id))
        }
        else{
          this.likeService.likeIds.update(ids => [...ids, this.member().id])
        }
      }
    })
  }
}
