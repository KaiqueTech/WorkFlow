import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { LoginResponse } from '../models/LoginModel';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  private readonly API_URL = 'http://localhost:5020/api/auth';

  constructor(private http: HttpClient) {}

  login(login: LoginResponse) {
    return this.http.post<any>(`${this.API_URL}/login`, login).pipe(
      tap(res => {
        localStorage.setItem('token', res.token);
      })
    );
  }

  getUserRole(): string {
    const token = localStorage.getItem('token');
    if (!token) return '';

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      
      const role = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || payload.role;
      
      return role ? role.toString().toLowerCase() : '';
    } catch {
      return '';
    }
  }

  isManager(): boolean {
    const role = this.getUserRole();
    return role === 'manager';
  }

  isUser(): boolean {
    const role = this.getUserRole();
    return role === 'user'
  }
}