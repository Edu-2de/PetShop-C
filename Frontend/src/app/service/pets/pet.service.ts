import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pet } from '../../model/pet.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PetService {
  private apiUrl = `${environment.apiUrl}/Animal`;

  constructor(private http: HttpClient) { }

  listar(): Observable<Pet[]> {
    return this.http.get<Pet[]>(this.apiUrl);
  }

  buscarPorId(id: number): Observable<Pet> {
    return this.http.get<Pet>(`${this.apiUrl}/${id}`);
  }

  buscarPorTutor(tutorId: number): Observable<Pet[]> {
    return this.http.get<Pet[]>(`${this.apiUrl}/tutor/${tutorId}`);
  }

  criar(pet: Omit<Pet, 'animalId' | 'tutorNome'>): Observable<Pet> {
    return this.http.post<Pet>(this.apiUrl, pet);
  }

  atualizar(id: number, pet: Omit<Pet, 'animalId' | 'tutorNome'>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, pet);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
