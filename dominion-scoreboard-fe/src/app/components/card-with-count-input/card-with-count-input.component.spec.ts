import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardWithCountInputComponent } from './card-with-count-input.component';

describe('CardWithCountInputComponent', () => {
  let component: CardWithCountInputComponent;
  let fixture: ComponentFixture<CardWithCountInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardWithCountInputComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardWithCountInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
