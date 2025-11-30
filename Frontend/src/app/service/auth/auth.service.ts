import { Injectable, signal, inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

export interface User {
  funcionarioId: number;
  nome: string;
  email: string;
  cargo: string;
}

interface LoginResponse {
  token: string;
  usuario: User;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = `${environment.apiUrl}/Auth/login`;

  // Signals para estado reativo
  private currentUserSignal = signal<User | null>(null);

  private readonly USER_KEY = 'sigapet_user';
  private readonly TOKEN_KEY = 'sigapet_token';

  constructor() {
    this.loadUserFromStorage();
  }

  // Método agora retorna um Observable para o componente tratar erro/sucesso
  login(email: string, senha: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.apiUrl, { email, senha }).pipe(
      tap(response => {
        this.saveToken(response.token);
        this.setCurrentUser(response.usuario);
      })
    );
  }

  logout(): void {
    this.currentUserSignal.set(null);
    localStorage.removeItem(this.USER_KEY);
    localStorage.removeItem(this.TOKEN_KEY);
    this.router.navigate(['/login']);
  }

  private setCurrentUser(user: User): void {
    this.currentUserSignal.set(user);
    localStorage.setItem(this.USER_KEY, JSON.stringify(user));
  }

  private saveToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  private loadUserFromStorage(): void {
    const userJson = localStorage.getItem(this.USER_KEY);
    if (userJson) {
      try {
        this.currentUserSignal.set(JSON.parse(userJson));
      } catch {
        this.logout();
      }
    }
  }

  getCurrentUser() {
    return this.currentUserSignal.asReadonly();
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    return !!this.getToken(); // Verifica se tem token
  }

  // Adaptar lógica de isAdmin baseada no Cargo que vem do banco
  isAdmin(): boolean {
    const user = this.currentUserSignal();

    // Se não tem usuário ou não tem cargo, é visitante ou cliente
    if (!user || !user.cargo) {
      return false;
    }

    // Normaliza para minúsculo para evitar erros de digitação (ex: "Gerente" vs "gerente")
    const cargo = user.cargo.toLowerCase().trim();

    // Lista de cargos que têm permissão de ver o menu "Gerenciar"
    // Adicionei todos os cargos que vi no seu FuncionarioFormComponent
    const cargosAdministrativos = [
      'gerente',
      'administrador',
      'admin',
      'veterinário',
      'veterinario', // sem acento por garantia
      'atendente',
      'tosador'
    ];

    return cargosAdministrativos.includes(cargo);
  }

  isUser(): boolean {
    return !!this.currentUserSignal();
  }
}
