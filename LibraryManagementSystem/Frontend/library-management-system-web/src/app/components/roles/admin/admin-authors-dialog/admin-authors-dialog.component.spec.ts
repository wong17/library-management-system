import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAuthorsDialogComponent } from './admin-authors-dialog.component';

describe('AdminAuthorsDialogComponent', () => {
  let component: AdminAuthorsDialogComponent;
  let fixture: ComponentFixture<AdminAuthorsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminAuthorsDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminAuthorsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
