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

  constructor(private formBuilder: FormBuilder, private userService: UserService) {
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
        console.log(response);
      },
      error: error => {
        console.log(error);
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
