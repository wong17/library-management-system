import { Component } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../../../services/security/user.service';
import { ToastrService } from 'ngx-toastr';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-student-home',
  standalone: true,
  imports: [MatSidenavModule, MatListModule, MatIconModule, MatToolbarModule, MatButtonModule, RouterModule, MatMenuModule],
  templateUrl: './student-home.component.html',
  styleUrl: './student-home.component.css'
})
export class StudentHomeComponent {

  menuItems = [
    { label: 'Libros', link: '/student-home/student-books', icon: 'book' },
    { label: 'Monografías', link: '/student-home/student-monographs', icon: 'description' }
  ];

  constructor(
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  logout() {
    this.userService.logout();
    this.router.navigate(['/login']);
    this.toastr.success('Cerró sesión exitosamente', 'Success', {
      timeOut: 5000
    });
  }

}
