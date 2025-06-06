import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardDisplayComponent } from './card-display.component';

describe('CardDisplayComponent', () => {
  let component: CardDisplayComponent;
  let fixture: ComponentFixture<CardDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardDisplayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
