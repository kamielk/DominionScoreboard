import { Component, Input } from '@angular/core';
import { Card } from '../../models/card.model';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-card-display',
  imports: [CommonModule],
  templateUrl: './card-display.component.html',
  styleUrl: './card-display.component.scss'
})
export class CardDisplayComponent {
  @Input() card!: Card;

  getBackgroundImageUrl(): string {
    return this.card?.imageUrl ? `url(${environment.apiUrl}/${this.card.imageUrl})` : 'none';
  }
}
