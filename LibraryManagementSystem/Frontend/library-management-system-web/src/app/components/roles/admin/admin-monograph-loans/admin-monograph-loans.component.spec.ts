import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminMonographLoansComponent } from './admin-monograph-loans.component';

describe('AdminMonographLoansComponent', () => {
  let component: AdminMonographLoansComponent;
  let fixture: ComponentFixture<AdminMonographLoansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminMonographLoansComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminMonographLoansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
