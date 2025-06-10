import { Component, Input } from '@angular/core';
import { Card } from '../../models/card.model';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-card-display',
  imports: [CommonModule],
  templateUrl: './card-display.component.html',
  styleUrl: './card-display.component.scss'
})
export class CardDisplayComponent {
  constructor(private sanitizer: DomSanitizer) {}

  @Input() card!: Card;

  getBackgroundImageUrl(): SafeUrl {
    return this.sanitizer.bypassSecurityTrustUrl(this.card?.imageUrl ? `${environment.apiUrl}${this.card.imageUrl}` : 'none');
  }
}
