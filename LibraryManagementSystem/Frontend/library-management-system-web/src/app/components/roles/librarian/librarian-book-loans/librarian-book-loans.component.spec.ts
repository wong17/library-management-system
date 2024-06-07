import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibrarianBookLoansComponent } from './librarian-book-loans.component';

describe('LibrarianBookLoansComponent', () => {
  let component: LibrarianBookLoansComponent;
  let fixture: ComponentFixture<LibrarianBookLoansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibrarianBookLoansComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LibrarianBookLoansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
