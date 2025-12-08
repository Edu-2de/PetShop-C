import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ServicoPet, CreateServicoPet, UpdateServicoPet, FuncionarioSimples } from '../../model/servico-pet.model';

@Injectable({
  providedIn: 'root'
})
export class ServicoPetService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Servico`;

  listar(): Observable<ServicoPet[]> {
    return this.http.get<ServicoPet[]>(this.apiUrl);
  }

  listarAtivos(): Observable<ServicoPet[]> {
    return this.http.get<ServicoPet[]>(`${this.apiUrl}/ativos`);
  }

  buscarPorId(id: number): Observable<ServicoPet> {
    return this.http.get<ServicoPet>(`${this.apiUrl}/${id}`);
  }

  // NOVO: Buscar funcionários aptos baseado nos cargos do serviço
  buscarFuncionariosAptos(servicoId: number): Observable<FuncionarioSimples[]> {
    return this.http.get<FuncionarioSimples[]>(`${this.apiUrl}/${servicoId}/funcionarios-aptos`);
  }

  // NOVO: Listar cargos disponíveis
  listarCargosDisponiveis(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/cargos-disponiveis`);
  }

  criar(servico: CreateServicoPet): Observable<ServicoPet> {
    return this.http.post<ServicoPet>(this.apiUrl, servico);
  }

  atualizar(id: number, servico: UpdateServicoPet): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, servico);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
