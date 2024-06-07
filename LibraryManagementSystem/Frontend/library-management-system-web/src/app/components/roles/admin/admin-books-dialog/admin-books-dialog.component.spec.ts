import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminBooksDialogComponent } from './admin-books-dialog.component';

describe('AdminBooksDialogComponent', () => {
  let component: AdminBooksDialogComponent;
  let fixture: ComponentFixture<AdminBooksDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminBooksDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminBooksDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
