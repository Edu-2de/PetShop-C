import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Tutor } from '../../model/tutor.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TutorService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Tutor`;

  findAll(): Observable<Tutor[]> {
    return this.http.get<Tutor[]>(this.apiUrl).pipe(
      map(tutores => tutores.map(t => ({ ...t, id: t.tutorId })))
    );
  }

  listar(): Observable<Tutor[]> {
    return this.findAll();
  }

  buscarPorId(id: number): Observable<Tutor> {
    return this.http.get<Tutor>(`${this.apiUrl}/${id}`).pipe(
      map(t => ({ ...t, id: t.tutorId }))
    );
  }

  criar(tutor: Omit<Tutor, 'tutorId' | 'id' | 'dataCadastro'>): Observable<Tutor> {
    return this.http.post<Tutor>(this.apiUrl, tutor).pipe(
      map(t => ({ ...t, id: t.tutorId }))
    );
  }

  atualizar(id: number, tutor: Omit<Tutor, 'tutorId' | 'id' | 'dataCadastro'>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, tutor);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
