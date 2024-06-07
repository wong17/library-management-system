import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibrarianBookLoansDialogComponent } from './librarian-book-loans-dialog.component';

describe('LibrarianBookLoansDialogComponent', () => {
  let component: LibrarianBookLoansDialogComponent;
  let fixture: ComponentFixture<LibrarianBookLoansDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibrarianBookLoansDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LibrarianBookLoansDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
