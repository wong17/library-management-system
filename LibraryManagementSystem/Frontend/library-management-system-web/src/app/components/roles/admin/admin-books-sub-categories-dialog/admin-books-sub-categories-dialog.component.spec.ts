import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminBooksSubCategoriesDialogComponent } from './admin-books-sub-categories-dialog.component';

describe('AdminBooksSubCategoriesDialogComponent', () => {
  let component: AdminBooksSubCategoriesDialogComponent;
  let fixture: ComponentFixture<AdminBooksSubCategoriesDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminBooksSubCategoriesDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminBooksSubCategoriesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
