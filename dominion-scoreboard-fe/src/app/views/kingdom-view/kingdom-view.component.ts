import { Component, OnInit } from '@angular/core';
import { Card } from '../../models/card.model';
import { CardDisplayComponent } from "../../components/card-display/card-display.component";
import { KingdomService } from '../../services/kingdom.service';
import { MatGridListModule } from '@angular/material/grid-list';

@Component({
  selector: 'app-kingdom-view',
  imports: [CardDisplayComponent, MatGridListModule],
  templateUrl: './kingdom-view.component.html',
  styleUrl: './kingdom-view.component.scss'
})
export class KingdomViewComponent implements OnInit {
  kingdom: Card[] = [];
  loading = false;
  
  constructor(private kingdomService: KingdomService) { }

  ngOnInit(): void {
    this.loadKingdom();
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

}
