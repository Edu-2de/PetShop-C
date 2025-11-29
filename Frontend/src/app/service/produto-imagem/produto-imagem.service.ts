import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ProdutoImagem } from '../../model/produto.model';

@Injectable({
  providedIn: 'root'
})
export class ProdutoImagemService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/ProdutoImagem`;

  uploadImagem(produtoId: number, file: File): Observable<ProdutoImagem> {
    const formData = new FormData();
    formData.append('file', file);
    
    return this.http.post<ProdutoImagem>(`${this.apiUrl}/${produtoId}/upload`, formData);
  }

  deleteImagem(imagemId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${imagemId}`);
  }
}
