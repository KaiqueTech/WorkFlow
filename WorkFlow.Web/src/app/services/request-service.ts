import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface RequestPayload {
  title: string;
  category: string;
  priority: string;
  description: string;
}

@Injectable({
  providedIn: 'root'
})
export class RequestService {
  // Verifique se a porta é 5020 ou 5000 no seu Program.cs
  private readonly API = 'http://localhost:5020/api/request'; 

  constructor(private http: HttpClient) {}

  // O Controller usa [HttpGet("search")] para a lista filtrada
  getRequests(filters: any = {}): Observable<any[]> {
    return this.http.get<any[]>(`${this.API}/search`, { params: filters });
  }

  // O Controller espera um Guid: {id:guid}
  getById(id: string): Observable<any> {
    return this.http.get<any>(`${this.API}/${id}`);
  }

  // Rota para o histórico da Timeline: [HttpGet("{id:guid}/history")]
  getHistory(id: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.API}/${id}/history`);
  }

  create(payload: any): Observable<any> {
    return this.http.post(this.API, payload);
  }

  update(id: string, payload: any): Observable<any> {
    return this.http.put(`${this.API}/${id}`, payload);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.API}/${id}`);
  }

  // Rota de Aprovação: [HttpPost("{id:guid}/approve")]
  approveRequest(id: string, comment: string): Observable<any> {
    return this.http.post(`${this.API}/${id}/approve`, { comment });
  }

  // Rota de Rejeição: [HttpPost("{id:guid}/reject")]
  rejectRequest(id: string, comment: string): Observable<any> {
    return this.http.post(`${this.API}/${id}/reject`, { comment });
  }
}