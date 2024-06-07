import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminBookLoansComponent } from './admin-book-loans.component';

describe('AdminBookLoansComponent', () => {
  let component: AdminBookLoansComponent;
  let fixture: ComponentFixture<AdminBookLoansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminBookLoansComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminBookLoansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
