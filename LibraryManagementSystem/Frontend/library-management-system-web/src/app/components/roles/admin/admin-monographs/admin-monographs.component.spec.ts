import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminMonographsComponent } from './admin-monographs.component';

describe('AdminMonographsComponent', () => {
  let component: AdminMonographsComponent;
  let fixture: ComponentFixture<AdminMonographsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminMonographsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminMonographsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
