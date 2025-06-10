import { Component, OnInit } from '@angular/core';
import { Card } from '../../models/card.model';
import { KingdomService } from '../../services/kingdom.service';
import { MatGridListModule } from '@angular/material/grid-list';
import { CardWithCountInputComponent } from "../../components/card-with-count-input/card-with-count-input.component";

@Component({
  selector: 'app-kingdom-view',
  imports: [MatGridListModule, CardWithCountInputComponent],
  templateUrl: './kingdom-view.component.html',
  styleUrl: './kingdom-view.component.scss'
})
export class KingdomViewComponent implements OnInit {
  baseCards: Card[] = [];
  kingdom: Card[] = [];
  loading = false;

  countsByName: Map<string, number> = new Map<string, number>();
  points: number = 0;
  
  constructor(private kingdomService: KingdomService) {}

  ngOnInit(): void {
    this.getBaseCards();
    this.loadKingdom();
  }

  getBaseCards() {
    this.kingdomService.getBaseCards().subscribe({
      next: (cards) => {
        this.baseCards = cards;
      },
      error: (error: Error) => {
        throw error;
      },
    })
  }

  loadKingdom() {
    this.loading = true;
    this.kingdomService.getRandomKingdom().subscribe({
      next: (cards) => {
        this.kingdom = cards;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      },
    })
  }

  handleCountChanged(card: Card, newCount: number) {
    this.countsByName.set(card.name, newCount)
  }

  calculateVictoryPoints() {
    this.kingdomService.calculateVictoryPoints(this.countsByName).subscribe({
      next: (response) => {
        this.points = response.victoryPoints;
      },
      error: (error: Error) => {
        throw error;
      }
    })
  }
}
