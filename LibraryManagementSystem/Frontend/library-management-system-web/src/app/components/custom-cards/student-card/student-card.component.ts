import { Component, Input } from '@angular/core';
import { StudentDto } from '../../../entities/dtos/university/student-dto';

@Component({
  selector: 'app-student-card',
  standalone: true,
  imports: [],
  templateUrl: './student-card.component.html',
  styleUrl: './student-card.component.css'
})
export class StudentCardComponent {

  @Input() student: StudentDto | undefined;

}
