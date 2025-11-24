import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ServicoPet } from '../../model/servico-pet.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ServicoPetService {
  private apiUrl = `${environment.apiUrl}/Servico`;

  constructor(private http: HttpClient) { }

  listar(): Observable<ServicoPet[]> {
    return this.http.get<ServicoPet[]>(this.apiUrl);
  }

  buscarPorId(id: number): Observable<ServicoPet> {
    return this.http.get<ServicoPet>(`${this.apiUrl}/${id}`);
  }

  listarAtivos(): Observable<ServicoPet[]> {
    return this.http.get<ServicoPet[]>(`${this.apiUrl}/ativos`);
  }

  criar(servico: Omit<ServicoPet, 'servicoId'>): Observable<ServicoPet> {
    return this.http.post<ServicoPet>(this.apiUrl, servico);
  }

  atualizar(id: number, servico: Omit<ServicoPet, 'servicoId'>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, servico);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
