import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from "./components/login/login.component";

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [RouterOutlet, LoginComponent]
})
export class AppComponent {
  title = 'Gestión de biblioteca';
  /* 
  Librerias de componentes a usar
  https://getbootstrap.com/
  https://ng-bootstrap.github.io/#/home
  https://valor-software.com/ngx-bootstrap/#/
  https://material.angular.io/
  */
}
