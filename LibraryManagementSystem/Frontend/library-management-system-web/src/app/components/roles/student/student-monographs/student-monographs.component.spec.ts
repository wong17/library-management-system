import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentMonographsComponent } from './student-monographs.component';

describe('StudentMonographsComponent', () => {
  let component: StudentMonographsComponent;
  let fixture: ComponentFixture<StudentMonographsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentMonographsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StudentMonographsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
