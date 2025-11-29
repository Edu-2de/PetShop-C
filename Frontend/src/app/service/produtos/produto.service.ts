import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Produto } from '../../model/produto.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProdutoService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/Produto`;

  findAll(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.apiUrl).pipe(
      map(produtos => produtos.map(p => ({
        ...p,
        id: p.produtoId,
        // Garante compatibilidade com alias
        fornecedorid: p.fornecedorId
      })))
    );
  }

  listar(): Observable<Produto[]> {
    return this.findAll();
  }

  findById(id: number): Observable<Produto> {
    return this.http.get<Produto>(`${this.apiUrl}/${id}`).pipe(
      map(p => ({
        ...p,
        id: p.produtoId,
        fornecedorid: p.fornecedorId
      }))
    );
  }

  searchByName(name: string): Observable<Produto[]> {
    return this.http.get<Produto[]>(`${this.apiUrl}/search?name=${name}`).pipe(
      map(produtos => produtos.map(p => ({
        ...p,
        id: p.produtoId,
        fornecedorid: p.fornecedorId
      })))
    );
  }

  create(produto: Partial<Produto>): Observable<Produto> {
    // Casting seguro para evitar erro de propriedade inexistente
    const pAny = produto as any;
    const fornecedorId = produto.fornecedorId || pAny.fornecedorid;
    const payload = { ...produto, fornecedorId };

    return this.http.post<Produto>(this.apiUrl, payload).pipe(
      map(p => ({ ...p, id: p.produtoId }))
    );
  }

  update(id: number, produto: Partial<Produto>): Observable<void> {
    const pAny = produto as any;
    const fornecedorId = produto.fornecedorId || pAny.fornecedorid;
    const payload = { ...produto, fornecedorId };

    return this.http.put<void>(`${this.apiUrl}/${id}`, payload);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
