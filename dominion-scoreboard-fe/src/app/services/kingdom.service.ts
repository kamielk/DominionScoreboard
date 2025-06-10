import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Card } from '../models/card.model';
import { Observable } from 'rxjs';
import { VictoryPointsResponse } from './models/victory-points-response';

@Injectable({ providedIn: 'root' })
export class KingdomService {
  constructor(private http: HttpClient) { }

  getRandomKingdom() : Observable<Card[]> {
    return this.http.get<Card[]>('/kingdom');
  }

  calculateVictoryPoints(countsByName: Map<string, number>) : Observable<VictoryPointsResponse> {
    return this.http.post<VictoryPointsResponse>('/calculate-vp', Object.fromEntries(countsByName));
  }
}


