import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibrarianMonographsComponent } from './librarian-monographs.component';

describe('LibrarianMonographsComponent', () => {
  let component: LibrarianMonographsComponent;
  let fixture: ComponentFixture<LibrarianMonographsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LibrarianMonographsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LibrarianMonographsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
