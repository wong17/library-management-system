import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentBooksLoansDialogComponent } from './student-books-loans-dialog.component';

describe('StudentBooksLoansDialogComponent', () => {
  let component: StudentBooksLoansDialogComponent;
  let fixture: ComponentFixture<StudentBooksLoansDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentBooksLoansDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StudentBooksLoansDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
