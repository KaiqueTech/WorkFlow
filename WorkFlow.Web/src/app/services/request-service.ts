import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateRequest } from '../models/CreateRequestModel';

@Injectable({
  providedIn: 'root'
})
export class RequestService {
  private readonly API = 'http://localhost:5020/api/request'; 

  constructor(private http: HttpClient) {}

getRequests(termo?: string, category?: string, prioridade?: string, status?: string, page: number = 0): Observable<any> {
  let params = new HttpParams()
    .set('Page', (page + 1).toString())
    .set('PageSize', '10');

  if (termo) params = params.set('SearchText', termo);
  if (status) params = params.set('Status', status);
  if (prioridade) params = params.set('Priority', prioridade);
  if (category && category !== '0') params = params.set('Category', category);

  return this.http.get<any>(`${this.API}/search`, { params });
}

  getById(id: string): Observable<any> {
    return this.http.get<any>(`${this.API}/${id}`);
  }

  getHistory(id: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.API}/${id}/history`);
  }

  create(request: CreateRequest): Observable<any> {
    return this.http.post(this.API, request);
  }
  approveRequest(id: string, comment: string): Observable<any> {
    return this.http.post(`${this.API}/${id}/approve`, { comment });
  }

  rejectRequest(id: string, comment: string): Observable<any> {
    return this.http.post(`${this.API}/${id}/reject`, { comment });
  }
}