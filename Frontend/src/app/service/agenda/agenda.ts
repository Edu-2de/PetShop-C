import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Agenda } from '../../model/agenda.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AgendaService {
  private apiUrl = `${environment.apiUrl}/Agendamento`;

  constructor(private http: HttpClient) { }

  listar(): Observable<Agenda[]> {
    return this.http.get<Agenda[]>(this.apiUrl);
  }

  buscarPorId(id: number): Observable<Agenda> {
    return this.http.get<Agenda>(`${this.apiUrl}/${id}`);
  }

  buscarPorAnimal(animalId: number): Observable<Agenda[]> {
    return this.http.get<Agenda[]>(`${this.apiUrl}/animal/${animalId}`);
  }

  buscarPorData(data: string): Observable<Agenda[]> {
    return this.http.get<Agenda[]>(`${this.apiUrl}/data/${data}`);
  }

  criar(agenda: Omit<Agenda, 'agendamentoId' | 'animalNome' | 'servicoNome' | 'funcionarioNome'>): Observable<Agenda> {
    return this.http.post<Agenda>(this.apiUrl, agenda);
  }

  atualizar(id: number, agenda: Omit<Agenda, 'agendamentoId' | 'animalNome' | 'servicoNome' | 'funcionarioNome'>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, agenda);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
