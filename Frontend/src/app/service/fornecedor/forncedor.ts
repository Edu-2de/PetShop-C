import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Fornecedor } from '../../model/fornecedor.model';
import { environment } from '../../../environments/environment'; // Importar environment

@Injectable({
  providedIn: 'root'
})
export class FornecedorService {
  private readonly http = inject(HttpClient);
  // CORREÇÃO: Usar a URL da API real
  private readonly apiUrl = `${environment.apiUrl}/Fornecedor`;

  findAll(): Observable<Fornecedor[]> {
    return this.http.get<Fornecedor[]>(this.apiUrl).pipe(
      // Mapeamento para compatibilidade caso o backend retorne FornecedorId
      map(data => data.map(f => ({
        ...f,
        id: f.fornecedorId // Garante que o front tenha um 'id' acessível
      })))
    );
  }

  listar(): Observable<Fornecedor[]> {
    return this.findAll();
  }

  findById(id: number): Observable<Fornecedor> {
    return this.http.get<Fornecedor>(`${this.apiUrl}/${id}`).pipe(
      map(f => ({ ...f, id: f.fornecedorId }))
    );
  }

  create(fornecedor: Partial<Fornecedor>): Observable<Fornecedor> {
    // O backend espera 'Nome', 'Email', etc.
    return this.http.post<Fornecedor>(this.apiUrl, fornecedor);
  }

  update(id: number, fornecedor: Partial<Fornecedor>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, fornecedor);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
