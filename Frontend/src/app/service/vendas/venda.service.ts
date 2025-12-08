import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Venda, CreateVendaDto } from '../../model/venda.model';

@Injectable({
  providedIn: 'root'
})
export class VendaService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Venda`; // CORRIGIDO: era /vendas, agora é /Venda

  criar(venda: CreateVendaDto): Observable<Venda> {
    return this.http.post<Venda>(this.apiUrl, venda);
  }

  // This method can be removed or kept for other purposes if needed.
  buscarPorTutor(tutorId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/tutor/${tutorId}`);
  }

  // NOVO: Buscar vendas pelo ID do usuário.
  buscarPorUsuario(usuarioId: number): Observable<Venda[]> {
    return this.http.get<Venda[]>(`${this.apiUrl}/usuario/${usuarioId}`);
  }

  listar(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  buscarPorId(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  atualizar(id: number, venda: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, venda);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // NOVO: Listar todas as vendas (para filtrar por usuário no frontend)
  listarTodas(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
