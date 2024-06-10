import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminMonographsAuthorsDialogComponent } from './admin-monographs-authors-dialog.component';

describe('AdminMonographsAuthorsDialogComponent', () => {
  let component: AdminMonographsAuthorsDialogComponent;
  let fixture: ComponentFixture<AdminMonographsAuthorsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminMonographsAuthorsDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminMonographsAuthorsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
