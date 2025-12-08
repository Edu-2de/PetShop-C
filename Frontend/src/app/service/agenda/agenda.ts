import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Agenda } from '../../model/agenda.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AgendaService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Agendamento`;

  findAll(): Observable<Agenda[]> {
    return this.http.get<any[]>(this.apiUrl).pipe(
      map(agendas => agendas.map(a => this.mapAgendamento(a)))
    );
  }

  listar(): Observable<Agenda[]> {
    return this.findAll();
  }

  buscarPorId(id: number): Observable<Agenda> {
    return this.http.get<any>(`${this.apiUrl}/${id}`).pipe(
      map(a => this.mapAgendamento(a))
    );
  }

  buscarPorTutor(tutorId: number): Observable<Agenda[]> {
    return this.http.get<any[]>(`${this.apiUrl}/tutor/${tutorId}`).pipe(
      map(agendas => agendas.map(a => this.mapAgendamento(a)))
    );
  }

  buscarPorUsuario(usuarioId: number): Observable<Agenda[]> {
    return this.http.get<any[]>(`${this.apiUrl}/usuario/${usuarioId}`).pipe(
      map(agendas => agendas.map(a => this.mapAgendamento(a)))
    );
  }

  criar(agenda: Partial<Agenda>): Observable<Agenda> {
    const payload = { 
      ...agenda, 
      animalId: agenda.animalId || agenda.petid,
      dataHora: agenda.dataHora || agenda.data
    };
    return this.http.post<any>(this.apiUrl, payload).pipe(
      map(a => this.mapAgendamento(a))
    );
  }

  atualizar(id: number, agenda: Partial<Agenda>): Observable<void> {
    const payload = { 
      ...agenda, 
      animalId: agenda.animalId || agenda.petid,
      dataHora: agenda.dataHora || agenda.data
    };
    return this.http.put<void>(`${this.apiUrl}/${id}`, payload);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  cancelar(id: number): Observable<void> {
    // Você pode criar um endpoint específico PATCH ou usar o PUT atualizando o status
    // Aqui assumindo que vamos carregar, mudar status e salvar via PUT
    // O ideal seria um endpoint PATCH /cancelar, mas vamos usar a lógica do componente por enquanto
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // 🆕 NOVO: Criar agendamento completo (com tutor e animal)
  criarCompleto(agendamentoCompleto: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/completo`, agendamentoCompleto).pipe(
      map(a => this.mapAgendamento(a))
    );
  }

  // ✅ Método auxiliar para mapear agendamento com todos os aliases necessários
  private mapAgendamento(a: any): Agenda {
    return {
      ...a,
      id: a.agendamentoId,
      petid: a.animalId,
      data: a.dataHora,
      // Mapear objeto pet se vier do backend
      pet: a.animal ? {
        animalId: a.animal.animalId,
        nome: a.animal.nome || a.animalNome,
        especie: a.animal.especie,
        raca: a.animal.raca,
        tutorId: a.animal.tutorId
      } : (a.animalNome ? {
        animalId: a.animalId,
        nome: a.animalNome,
        especie: '',
        raca: '',
        tutorId: 0
      } : undefined),
      // Mapear objeto servico se vier do backend
      servico: a.servico ? {
        servicoId: a.servico.servicoId,
        nome: a.servico.nome || a.servicoNome,
        preco: a.servico.preco,
        duracaoMinutos: a.servico.duracaoMinutos
      } : (a.servicoNome ? {
        servicoId: a.servicoId,
        nome: a.servicoNome,
        preco: 0,
        duracaoMinutos: 0
      } : undefined),
      // Mapear objeto funcionario se vier do backend
      funcionario: a.funcionario ? {
        funcionarioId: a.funcionario.funcionarioId,
        nome: a.funcionario.nome || a.funcionarioNome,
        cargo: a.funcionario.cargo
      } : (a.funcionarioNome ? {
        funcionarioId: a.funcionarioId!,
        nome: a.funcionarioNome,
        cargo: ''
      } : undefined)
    };
  }
}
