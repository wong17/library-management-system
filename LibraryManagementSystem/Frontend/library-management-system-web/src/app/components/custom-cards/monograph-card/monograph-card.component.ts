import { Component, Input } from '@angular/core';
import { MonographDto } from '../../../entities/dtos/library/monograph-dto';
import { MatChipsModule } from '@angular/material/chips';

@Component({
  selector: 'app-monograph-card',
  standalone: true,
  imports: [MatChipsModule],
  templateUrl: './monograph-card.component.html',
  styleUrl: './monograph-card.component.css'
})
export class MonographCardComponent {

  @Input() monograph: MonographDto | undefined;

}
