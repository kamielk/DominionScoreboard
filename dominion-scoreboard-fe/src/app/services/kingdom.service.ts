import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Card } from '../models/card.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class KingdomService {
  constructor(private http: HttpClient) { }

  getRandomKingdom() : Observable<Card[]> {
    return this.http.get<Card[]>('/kingdom');
  }
}
