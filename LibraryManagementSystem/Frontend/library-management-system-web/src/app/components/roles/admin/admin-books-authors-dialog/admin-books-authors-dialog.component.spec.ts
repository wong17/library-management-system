import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminBooksAuthorsDialogComponent } from './admin-books-authors-dialog.component';

describe('AdminBooksAuthorsDialogComponent', () => {
  let component: AdminBooksAuthorsDialogComponent;
  let fixture: ComponentFixture<AdminBooksAuthorsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminBooksAuthorsDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminBooksAuthorsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
