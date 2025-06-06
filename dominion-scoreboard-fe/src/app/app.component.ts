import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { KingdomViewComponent } from "./views/kingdom-view/kingdom-view.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, KingdomViewComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'dominion-scoreboard-fe';
}
