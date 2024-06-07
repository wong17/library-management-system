import { Component } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-librarian-home',
  standalone: true,
  imports: [MatSidenavModule, MatListModule, MatIconModule, MatToolbarModule, MatButtonModule, RouterModule],
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

}
