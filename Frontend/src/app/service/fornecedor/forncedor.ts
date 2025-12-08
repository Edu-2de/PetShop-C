import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Fornecedor } from '../../model/fornecedor.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FornecedorService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Fornecedor`;

  listar(): Observable<Fornecedor[]> {
    return this.http.get<Fornecedor[]>(this.apiUrl);
  }

  findById(id: number): Observable<Fornecedor> {
    return this.http.get<Fornecedor>(`${this.apiUrl}/${id}`);
  }

  create(fornecedor: Partial<Fornecedor>): Observable<Fornecedor> {
    return this.http.post<Fornecedor>(this.apiUrl, fornecedor);
  }

  update(id: number, fornecedor: Partial<Fornecedor>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, fornecedor);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
