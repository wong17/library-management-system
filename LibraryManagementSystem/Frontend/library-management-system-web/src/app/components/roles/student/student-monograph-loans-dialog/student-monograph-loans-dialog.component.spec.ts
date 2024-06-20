import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentMonographLoansDialogComponent } from './student-monograph-loans-dialog.component';

describe('StudentMonographLoansDialogComponent', () => {
  let component: StudentMonographLoansDialogComponent;
  let fixture: ComponentFixture<StudentMonographLoansDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentMonographLoansDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StudentMonographLoansDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
