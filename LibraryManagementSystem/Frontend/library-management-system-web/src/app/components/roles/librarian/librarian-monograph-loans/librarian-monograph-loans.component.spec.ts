import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibrarianMonographLoansComponent } from './librarian-monograph-loans.component';

describe('LibrarianMonographLoansComponent', () => {
  let component: LibrarianMonographLoansComponent;
  let fixture: ComponentFixture<LibrarianMonographLoansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibrarianMonographLoansComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LibrarianMonographLoansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
