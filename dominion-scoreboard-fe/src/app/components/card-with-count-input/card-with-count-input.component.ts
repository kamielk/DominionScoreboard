import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CardDisplayComponent } from "../card-display/card-display.component";
import { Card } from '../../models/card.model';

@Component({
  selector: 'app-card-with-count-input',
  imports: [CardDisplayComponent],
  templateUrl: './card-with-count-input.component.html',
  styleUrl: './card-with-count-input.component.scss'
})
export class CardWithCountInputComponent {
  @Input() card!: Card;
  @Output() onCountChanged = new EventEmitter<number>();

  onInputChanged(event: Event) {
    var input = event.target as HTMLInputElement;
    const parsed = parseInt(input.value, 10);    
    if (this.onCountChanged) {
      this.onCountChanged.emit(isNaN(parsed) ? 0 : parsed);
    }
  }
}
