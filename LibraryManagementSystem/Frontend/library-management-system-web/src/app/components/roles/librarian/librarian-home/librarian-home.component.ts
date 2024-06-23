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
  selector: 'app-librarian-home',
  standalone: true,
  imports: [MatSidenavModule, MatListModule, MatIconModule, MatToolbarModule, MatButtonModule, RouterModule, MatMenuModule],
  templateUrl: './librarian-home.component.html',
  styleUrl: './librarian-home.component.css'
})
export class LibrarianHomeComponent {

  menuItems = [
    { label: 'Dashboard', link: '/librarian-home/librarian-dashboard', icon: 'dashboard' },
    { label: 'Libros', link: '/librarian-home/librarian-books', icon: 'book' },
    { label: 'Monografías', link: '/librarian-home/librarian-monographs', icon: 'description' },
    { label: 'Préstamos de Libros', link: '/librarian-home/librarian-book-loans', icon: 'assignment' },
    { label: 'Préstamos de Monografías', link: '/librarian-home/librarian-monograph-loans', icon: 'assignment' }
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
