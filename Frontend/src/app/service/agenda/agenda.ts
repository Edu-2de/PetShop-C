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
    return this.http.get<Agenda[]>(this.apiUrl).pipe(
      map(agendas => agendas.map(a => ({ 
        ...a, 
        id: a.agendamentoId,
        petid: a.animalId,
        data: a.dataHora
      })))
    );
  }

  listar(): Observable<Agenda[]> {
    return this.findAll();
  }

  buscarPorId(id: number): Observable<Agenda> {
    return this.http.get<Agenda>(`${this.apiUrl}/${id}`).pipe(
      map(a => ({ 
        ...a, 
        id: a.agendamentoId,
        petid: a.animalId,
        data: a.dataHora
      }))
    );
  }

  criar(agenda: Partial<Agenda>): Observable<Agenda> {
    const payload = { 
      ...agenda, 
      animalId: agenda.animalId || agenda.petid,
      dataHora: agenda.dataHora || agenda.data
    };
    return this.http.post<Agenda>(this.apiUrl, payload).pipe(
      map(a => ({ 
        ...a, 
        id: a.agendamentoId,
        petid: a.animalId,
        data: a.dataHora
      }))
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
}
