import { Routes } from '@angular/router';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' }, //  Se activará cuando la URL sea la raíz de la aplicación (Redirige a LoginComponent por defecto)
    { path: 'login', component: LoginComponent },
    { path: 'home', component: HomeComponent },
    { path: '**', component: NotFoundComponent } // **: Este es un comodín que captura cualquier ruta que no coincida con las definidas anteriormente.
];
