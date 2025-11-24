import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Produto } from '../../model/produto.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProdutoService {
  private apiUrl = `${environment.apiUrl}/Produto`;

  constructor(private http: HttpClient) { }

  findAll(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.apiUrl);
  }

  findById(id: number): Observable<Produto> {
    return this.http.get<Produto>(`${this.apiUrl}/${id}`);
  }

  findAtivos(): Observable<Produto[]> {
    return this.http.get<Produto[]>(`${this.apiUrl}/ativos`);
  }

  create(produto: Omit<Produto, 'produtoId' | 'fornecedorNome'>): Observable<Produto> {
    return this.http.post<Produto>(this.apiUrl, produto);
  }

  update(id: number, produto: Omit<Produto, 'produtoId' | 'fornecedorNome'>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, produto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
