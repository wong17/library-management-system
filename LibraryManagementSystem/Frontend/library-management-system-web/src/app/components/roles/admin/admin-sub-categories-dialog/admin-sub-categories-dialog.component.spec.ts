import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminSubCategoriesDialogComponent } from './admin-sub-categories-dialog.component';

describe('AdminSubCategoriesDialogComponent', () => {
  let component: AdminSubCategoriesDialogComponent;
  let fixture: ComponentFixture<AdminSubCategoriesDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminSubCategoriesDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminSubCategoriesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
