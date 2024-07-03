import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookReturnDialogComponent } from './book-return-dialog.component';

describe('BookReturnDialogComponent', () => {
  let component: BookReturnDialogComponent;
  let fixture: ComponentFixture<BookReturnDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookReturnDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BookReturnDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
