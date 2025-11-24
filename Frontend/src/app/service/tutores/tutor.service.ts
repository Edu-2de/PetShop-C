import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tutor } from '../../model/tutor.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TutorService {
  private apiUrl = `${environment.apiUrl}/Tutor`;

  constructor(private http: HttpClient) { }

  listar(): Observable<Tutor[]> {
    return this.http.get<Tutor[]>(this.apiUrl);
  }

  buscarPorId(id: number): Observable<Tutor> {
    return this.http.get<Tutor>(`${this.apiUrl}/${id}`);
  }

  criar(tutor: Omit<Tutor, 'tutorId' | 'dataCadastro'>): Observable<Tutor> {
    return this.http.post<Tutor>(this.apiUrl, tutor);
  }

  atualizar(id: number, tutor: Omit<Tutor, 'tutorId' | 'dataCadastro'>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, tutor);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
