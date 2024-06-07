import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminMonographLoansDialogComponent } from './admin-monograph-loans-dialog.component';

describe('AdminMonographLoansDialogComponent', () => {
  let component: AdminMonographLoansDialogComponent;
  let fixture: ComponentFixture<AdminMonographLoansDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminMonographLoansDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminMonographLoansDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
