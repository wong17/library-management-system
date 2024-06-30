import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonographCardComponent } from './monograph-card.component';

describe('MonographCardComponent', () => {
  let component: MonographCardComponent;
  let fixture: ComponentFixture<MonographCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MonographCardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MonographCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
