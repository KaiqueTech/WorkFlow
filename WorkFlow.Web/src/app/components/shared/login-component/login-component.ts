import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginService } from '../../../services/login-service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login-component',
  imports: [ReactiveFormsModule],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css',
})
export class LoginComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(LoginService);
  private router = inject(Router);
  private message = inject(MatSnackBar);
  
  loginForm: FormGroup;

  constructor(
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (token) {
      this.router.navigate(['/home']);
    }
  }

  onSubmit() {
    if (this.loginForm.valid) {

      this.authService.login(this.loginForm.value).subscribe({
        next: (res: any) => {
          
          if (res.token) {
            localStorage.setItem('token', res.token);
            this.router.navigate(['/home']);
          }
          this.message.open('Login efetuado com sucesso!', 'Dismiss', {
            duration: 9000
          });
          this.loginForm.reset();
        },
        error: (err) => {
          console.error('Erro ao fazer login', err);
         
          this.message.open('E-mail ou senha incorretos. Tente novamente.', 'Dismiss', {
              duration: 9000
          });
          this.loginForm.reset();
        }
      });
    }
  }
}
