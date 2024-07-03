import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonographReturnDialogComponent } from './monograph-return-dialog.component';

describe('MonographReturnDialogComponent', () => {
  let component: MonographReturnDialogComponent;
  let fixture: ComponentFixture<MonographReturnDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MonographReturnDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MonographReturnDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
