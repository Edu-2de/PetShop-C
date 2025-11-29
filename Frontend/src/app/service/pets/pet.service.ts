import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Pet } from '../../model/pet.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PetService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Animal`;

  findAll(): Observable<Pet[]> {
    return this.http.get<Pet[]>(this.apiUrl).pipe(
      map(pets => pets.map(p => ({ 
        ...p, 
        id: p.animalId,
        nascimento: p.dataNascimento 
      })))
    );
  }

  listar(): Observable<Pet[]> {
    return this.findAll();
  }

  buscarPorId(id: number): Observable<Pet> {
    return this.http.get<Pet>(`${this.apiUrl}/${id}`).pipe(
      map(p => ({ 
        ...p, 
        id: p.animalId,
        nascimento: p.dataNascimento 
      }))
    );
  }

  searchByName(name: string): Observable<Pet[]> {
    return this.http.get<Pet[]>(`${this.apiUrl}/search?name=${name}`).pipe(
      map(pets => pets.map(p => ({
        ...p,
        id: p.animalId,
        nascimento: p.dataNascimento
      })))
    );
  }

  criar(pet: Partial<Pet>): Observable<Pet> {
    const payload = { ...pet, dataNascimento: pet.dataNascimento || pet.nascimento };
    return this.http.post<Pet>(this.apiUrl, payload).pipe(
      map(p => ({ 
        ...p, 
        id: p.animalId,
        nascimento: p.dataNascimento 
      }))
    );
  }

  atualizar(id: number, pet: Partial<Pet>): Observable<void> {
    const payload = { ...pet, dataNascimento: pet.dataNascimento || pet.nascimento };
    return this.http.put<void>(`${this.apiUrl}/${id}`, payload);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  buscarPorTutor(tutorId: number): Observable<Pet[]> {
    return this.http.get<Pet[]>(`${this.apiUrl}/tutor/${tutorId}`).pipe(
      map(pets => pets.map(p => ({ 
        ...p, 
        id: p.animalId,
        nascimento: p.dataNascimento 
      })))
    );
  }
}
