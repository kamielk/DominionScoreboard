import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KingdomViewComponent } from './kingdom-view.component';

describe('KingdomViewComponent', () => {
  let component: KingdomViewComponent;
  let fixture: ComponentFixture<KingdomViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [KingdomViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(KingdomViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
