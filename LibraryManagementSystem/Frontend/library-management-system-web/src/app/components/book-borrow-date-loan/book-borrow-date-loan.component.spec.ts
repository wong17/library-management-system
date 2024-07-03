import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookBorrowDateLoanComponent } from './book-borrow-date-loan.component';

describe('BookBorrowDateLoanComponent', () => {
  let component: BookBorrowDateLoanComponent;
  let fixture: ComponentFixture<BookBorrowDateLoanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookBorrowDateLoanComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BookBorrowDateLoanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
