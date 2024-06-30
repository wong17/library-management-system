import { Component, Input } from '@angular/core';
import { BookDto } from '../../../entities/dtos/library/book-dto';
import { MatChipsModule } from '@angular/material/chips';

@Component({
  selector: 'app-book-card',
  standalone: true,
  imports: [MatChipsModule],
  templateUrl: './book-card.component.html',
  styleUrl: './book-card.component.css'
})
export class BookCardComponent {
  
  @Input() book: BookDto | undefined;

}
