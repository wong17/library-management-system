import { Routes } from '@angular/router';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { LoginComponent } from './components/login/login.component';
import { AdminHomeComponent } from './components/roles/admin/admin-home/admin-home.component';
import { StudentHomeComponent } from './components/roles/student/student-home/student-home.component';
import { LibrarianHomeComponent } from './components/roles/librarian/librarian-home/librarian-home.component';
import { AdminDashboardComponent } from './components/roles/admin/admin-dashboard/admin-dashboard.component';
import { AdminBooksComponent } from './components/roles/admin/admin-books/admin-books.component';
import { AdminMonographsComponent } from './components/roles/admin/admin-monographs/admin-monographs.component';
import { AdminBookLoansComponent } from './components/roles/admin/admin-book-loans/admin-book-loans.component';
import { AdminPublishersComponent } from './components/roles/admin/admin-publishers/admin-publishers.component';
import { AdminAuthorsComponent } from './components/roles/admin/admin-authors/admin-authors.component';
import { AdminCategoriesComponent } from './components/roles/admin/admin-categories/admin-categories.component';
import { AdminSubCategoriesComponent } from './components/roles/admin/admin-sub-categories/admin-sub-categories.component';
import { AdminMonographLoansComponent } from './components/roles/admin/admin-monograph-loans/admin-monograph-loans.component';
import { StudentBooksComponent } from './components/roles/student/student-books/student-books.component';
import { StudentMonographsComponent } from './components/roles/student/student-monographs/student-monographs.component';
import { LibrarianDashboardComponent } from './components/roles/librarian/librarian-dashboard/librarian-dashboard.component';
import { LibrarianBooksComponent } from './components/roles/librarian/librarian-books/librarian-books.component';
import { LibrarianMonographsComponent } from './components/roles/librarian/librarian-monographs/librarian-monographs.component';
import { LibrarianBookLoansComponent } from './components/roles/librarian/librarian-book-loans/librarian-book-loans.component';
import { LibrarianMonographLoansComponent } from './components/roles/librarian/librarian-monograph-loans/librarian-monograph-loans.component';
import { AdminGuard, LibrarianGuard, StudentGuard } from './services/auth-guard';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' }, // Se activará cuando la URL sea la raíz de la aplicación (Redirige a LoginComponent por defecto)
    { path: 'login', component: LoginComponent },
    /* Componentes Admin */
    { path: 'admin-home', component: AdminHomeComponent, canActivate: [AdminGuard], children: [
        { path: '', redirectTo: 'admin-dashboard', pathMatch: 'full' }, // Redirige automáticamente a admin-dashboard al carga admin-home
        { path: 'admin-dashboard', component: AdminDashboardComponent },
        { path: 'admin-books', component: AdminBooksComponent },
        { path: 'admin-monographs', component: AdminMonographsComponent },
        { path: 'admin-book-loans', component: AdminBookLoansComponent },
        { path: 'admin-monograph-loans', component: AdminMonographLoansComponent },
        { path: 'admin-publishers', component: AdminPublishersComponent },
        { path: 'admin-authors', component: AdminAuthorsComponent },
        { path: 'admin-categories', component: AdminCategoriesComponent },
        { path: 'admin-sub-categories', component: AdminSubCategoriesComponent }
    ]},
    
    /* Componentes Bibliotecario */
    { path: 'librarian-home', component: LibrarianHomeComponent, canActivate: [LibrarianGuard], children: [
        { path: '', redirectTo: 'librarian-dashboard', pathMatch: 'full' }, // Redirige automáticamente a librarian-dashboard al carga librarian-home
        { path: 'librarian-dashboard', component: LibrarianDashboardComponent },
        { path: 'librarian-books', component: LibrarianBooksComponent },
        { path: 'librarian-monographs', component: LibrarianMonographsComponent },
        { path: 'librarian-book-loans', component: LibrarianBookLoansComponent },
        { path: 'librarian-monograph-loans', component: LibrarianMonographLoansComponent }
    ]},

    /* Componentes Estudiante */
    { path: 'student-home', component: StudentHomeComponent, canActivate: [StudentGuard], children: [
        { path: '', redirectTo: 'student-books', pathMatch: 'full' }, // Redirige automáticamente a student-books al carga student-home
        { path: 'student-books', component: StudentBooksComponent },
        { path: 'student-monographs', component: StudentMonographsComponent }
    ] },

    { path: '**', component: NotFoundComponent } // **: Este es un comodín que captura cualquier ruta que no coincida con las definidas anteriormente.
];
