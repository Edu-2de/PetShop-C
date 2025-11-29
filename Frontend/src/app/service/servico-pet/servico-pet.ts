import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { ServicoPet } from '../../model/servico-pet.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ServicoPetService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Servico`;

  findAll(): Observable<ServicoPet[]> {
    return this.http.get<ServicoPet[]>(this.apiUrl).pipe(
      map(servicos => servicos.map(s => ({ ...s, id: s.servicoId })))
    );
  }

  listar(): Observable<ServicoPet[]> {
    return this.findAll();
  }

  buscarPorId(id: number): Observable<ServicoPet> {
    return this.http.get<ServicoPet>(`${this.apiUrl}/${id}`).pipe(
      map(s => ({ ...s, id: s.servicoId }))
    );
  }

  searchByName(name: string): Observable<ServicoPet[]> {
    return this.http.get<ServicoPet[]>(`${this.apiUrl}/search?name=${name}`).pipe(
      map(servicos => servicos.map(s => ({ ...s, id: s.servicoId })))
    );
  }

  criar(servico: Partial<ServicoPet>): Observable<ServicoPet> {
    return this.http.post<ServicoPet>(this.apiUrl, servico).pipe(
      map(s => ({ ...s, id: s.servicoId }))
    );
  }

  atualizar(id: number, servico: Partial<ServicoPet>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, servico);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
