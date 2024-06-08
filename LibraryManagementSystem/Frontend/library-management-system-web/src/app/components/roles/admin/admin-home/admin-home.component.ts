import { Component } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin-home',
  standalone: true,
  imports: [MatSidenavModule, MatListModule, MatIconModule, MatToolbarModule, MatButtonModule, RouterModule],
  templateUrl: './admin-home.component.html',
  styleUrl: './admin-home.component.css'
})
export class AdminHomeComponent {

  menuItems = [
    { label: 'Dashboard', link: '/admin-home/admin-dashboard', icon: 'dashboard' },
    { label: 'Libros', link: '/admin-home/admin-books', icon: 'book' },
    { label: 'Monografías', link: '/admin-home/admin-monographs', icon: 'description' },
    { label: 'Préstamos de Libros', link: '/admin-home/admin-book-loans', icon: 'assignment' },
    { label: 'Préstamos de Monografías', link: '/admin-home/admin-monograph-loans', icon: 'assignment' },
    { label: 'Editoriales', link: '/admin-home/admin-publishers', icon: 'business' },
    { label: 'Autores', link: '/admin-home/admin-authors', icon: 'person' },
    { label: 'Categorías', link: '/admin-home/admin-categories', icon: 'category' },
    { label: 'Subcategorías', link: '/admin-home/admin-sub-categories', icon: 'subdirectory_arrow_right' }
  ];

}