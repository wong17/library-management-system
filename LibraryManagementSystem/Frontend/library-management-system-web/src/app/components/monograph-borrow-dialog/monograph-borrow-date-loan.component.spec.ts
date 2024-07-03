import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonographBorrowDateLoanComponent } from './monograph-borrow-date-loan.component';

describe('MonographBorrowDialogComponent', () => {
  let component: MonographBorrowDateLoanComponent;
  let fixture: ComponentFixture<MonographBorrowDateLoanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MonographBorrowDateLoanComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MonographBorrowDateLoanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
