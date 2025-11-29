import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface Funcionario {
  funcionarioId: number;
  nome: string;
  cargo: string;
  telefone: string;
  email: string;
  dataContratacao: Date;
  senha?: string; // Usado apenas na criação
}

@Injectable({ providedIn: 'root' })
export class FuncionarioService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Funcionario`;

  listar(): Observable<Funcionario[]> {
    return this.http.get<Funcionario[]>(this.apiUrl);
  }

  buscarPorId(id: number): Observable<Funcionario> {
    return this.http.get<Funcionario>(`${this.apiUrl}/${id}`);
  }

  criar(func: Funcionario): Observable<Funcionario> {
    return this.http.post<Funcionario>(this.apiUrl, func);
  }

  atualizar(id: number, func: Funcionario): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, func);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
