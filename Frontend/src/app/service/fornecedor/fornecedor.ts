import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Fornecedor } from '../../model/fornecedor.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FornecedorService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Fornecedor`;

  findAll(): Observable<Fornecedor[]> {
    return this.http.get<Fornecedor[]>(this.apiUrl).pipe(
      map(fornecedores => fornecedores.map(f => ({ 
        ...f, 
        id: f.fornecedorId,
        contato: f.telefone 
      })))
    );
  }

  listar(): Observable<Fornecedor[]> {
    return this.findAll();
  }

  findById(id: number): Observable<Fornecedor> {
    return this.http.get<Fornecedor>(`${this.apiUrl}/${id}`).pipe(
      map(f => ({ 
        ...f, 
        id: f.fornecedorId,
        contato: f.telefone 
      }))
    );
  }

  searchByName(name: string): Observable<Fornecedor[]> {
    return this.http.get<Fornecedor[]>(`${this.apiUrl}/search?name=${name}`).pipe(
      map(fornecedores => fornecedores.map(f => ({
        ...f,
        id: f.fornecedorId,
        contato: f.telefone
      })))
    );
  }

  create(fornecedor: Partial<Fornecedor>): Observable<Fornecedor> {
    const payload = { ...fornecedor, telefone: fornecedor.telefone || fornecedor.contato };
    return this.http.post<Fornecedor>(this.apiUrl, payload).pipe(
      map(f => ({ 
        ...f, 
        id: f.fornecedorId,
        contato: f.telefone 
      }))
    );
  }

  update(id: number, fornecedor: Partial<Fornecedor>): Observable<void> {
    const payload = { ...fornecedor, telefone: fornecedor.telefone || fornecedor.contato };
    return this.http.put<void>(`${this.apiUrl}/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
