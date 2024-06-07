import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCategoriesDialogComponent } from './admin-categories-dialog.component';

describe('AdminCategoriesDialogComponent', () => {
  let component: AdminCategoriesDialogComponent;
  let fixture: ComponentFixture<AdminCategoriesDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminCategoriesDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminCategoriesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
