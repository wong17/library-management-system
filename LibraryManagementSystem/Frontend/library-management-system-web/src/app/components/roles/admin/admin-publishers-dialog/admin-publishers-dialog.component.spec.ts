import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPublishersDialogComponent } from './admin-publishers-dialog.component';

describe('AdminPublishersDialogComponent', () => {
  let component: AdminPublishersDialogComponent;
  let fixture: ComponentFixture<AdminPublishersDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminPublishersDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminPublishersDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
