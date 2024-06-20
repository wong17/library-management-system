import { Component } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { ReactiveFormsModule } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import { ControlStateMatcher } from '../../util/control-state-matcher';
import { UserLogInDto } from '../../entities/dtos/security/user-log-in-dto';
import { UserService } from '../../services/security/user.service';
import { Router } from '@angular/router';
import { UserDto } from '../../entities/dtos/security/user-dto';
import { ApiResponse } from '../../entities/api/api-response';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, MatCardModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  /* Formulario de inicio de sesión */
  loginForm: FormGroup;
  /* */
  matcher: ControlStateMatcher = new ControlStateMatcher()
  /* Mapea un rol a un componente */
  private roleRoutes: { [key: string]: string } = {
    'ADMIN': '/admin-home',
    'STUDENT': '/student-home',
    'LIBRARIAN': '/librarian-home'
  };

  constructor(private formBuilder: FormBuilder, private userService: UserService, private toastr: ToastrService, private router: Router) {
    /* Agrupar controles, crear formulario y agregar validaciones */
    this.loginForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(64)]]
    });
  }

  onSubmit() {
    const userLoginDto: UserLogInDto = this.loginForm.value;
    this.userService.login(userLoginDto).subscribe({
      next: response => {
        // Comprobar si se pudo iniciar sesión
        if (response.isSuccess !== 0 && response.statusCode !== 200) {
          this.toastr.warning(`${response.message}`, 'Atención', {
            timeOut: 5000
          })
          return;
        }
        // Obtener rol del usuario
        var userDto: UserDto = response.result as UserDto;
        if (!userDto.roles || userDto.roles.length === 0) {
          this.toastr.warning(`${response.message}`, 'Atención', {
            timeOut: 5000
          })
          return;
        }
        // Redirigir según el rol del usuario 
        const role = userDto.roles[0].name;
        if (!role) {
          this.toastr.error(`${response.message}`, 'Error', {
            timeOut: 5000
          })
          return;
        }
        
        const route = this.roleRoutes[role];
        if (route) {
          this.router.navigate([route]);

          this.toastr.success(`${response.message}`, 'Exito', {
            timeOut: 5000
          })
        }

      },
      error: (error: ApiResponse) => {
        this.toastr.error(`${error.message}`, 'Error', {
          timeOut: 5000
        });
      }
    })
  }

  /* Getters */
  get username() {
    return this.loginForm.get('username');
  }

  get password() {
    return this.loginForm.get('password');
  }

}
