import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminMonographsDialogComponent } from './admin-monographs-dialog.component';

describe('AdminMonographsDialogComponent', () => {
  let component: AdminMonographsDialogComponent;
  let fixture: ComponentFixture<AdminMonographsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminMonographsDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminMonographsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
