import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibrarianMonographLoansDialogComponent } from './librarian-monograph-loans-dialog.component';

describe('LibrarianMonographLoansDialogComponent', () => {
  let component: LibrarianMonographLoansDialogComponent;
  let fixture: ComponentFixture<LibrarianMonographLoansDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibrarianMonographLoansDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LibrarianMonographLoansDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
