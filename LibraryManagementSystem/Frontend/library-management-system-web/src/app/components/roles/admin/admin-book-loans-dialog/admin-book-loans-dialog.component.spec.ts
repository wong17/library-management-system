import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminBookLoansDialogComponent } from './admin-book-loans-dialog.component';

describe('AdminBookLoansDialogComponent', () => {
  let component: AdminBookLoansDialogComponent;
  let fixture: ComponentFixture<AdminBookLoansDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminBookLoansDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminBookLoansDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
