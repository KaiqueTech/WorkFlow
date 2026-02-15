import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginService } from '../../../services/login-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-component',
  imports: [ReactiveFormsModule],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css',
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  errorMessage: string = ''; 

  constructor(
    private fb: FormBuilder,
    private authService: LoginService,
    private router: Router
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
      this.errorMessage = ''; 
      
      this.authService.login(this.loginForm.value).subscribe({
        next: (res: any) => {
          
          if (res.token) {
            localStorage.setItem('token', res.token);
            this.router.navigate(['/home']);
          }
        },
        error: (err) => {
          console.error('Erro ao fazer login', err);
         
          this.errorMessage = 'E-mail ou senha incorretos. Tente novamente.';
        }
      });
    }
  }
}
