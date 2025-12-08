import { Injectable, signal, inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

export interface User {
  usuarioId: number;
  nome: string;
  email: string;
  cargo: string;
  funcionarioId?: number;
  tutorId?: number;

  // Alias para compatibilidade
  id?: number;
}

interface LoginResponse {
  token: string;
  usuario: User;
}

interface RegisterData {
  nome: string;
  email: string;
  senha: string;
  telefone: string;
  endereco: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = `${environment.apiUrl}/Auth`;

  // Signals para estado reativo
  private currentUserSignal = signal<User | null>(null);

  private readonly USER_KEY = 'sigapet_user';
  private readonly TOKEN_KEY = 'sigapet_token';

  constructor() {
    this.loadUserFromStorage();
  }

  login(email: string, senha: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, { email, senha }).pipe(
      tap((response) => {
        this.saveToken(response.token);
        this.setCurrentUser({
          ...response.usuario,
          id:
            response.usuario.tutorId ||
            response.usuario.funcionarioId ||
            response.usuario.usuarioId,
        });
      })
    );
  }

  register(data: RegisterData): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/register`, data).pipe(
      tap((response) => {
        this.saveToken(response.token);
        this.setCurrentUser({
          ...response.usuario,
          id:
            response.usuario.tutorId ||
            response.usuario.funcionarioId ||
            response.usuario.usuarioId,
        });
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
        const user = JSON.parse(userJson);
        // Garantir compatibilidade com campo id
        if (!user.id && (user.tutorId || user.funcionarioId || user.usuarioId)) {
          user.id = user.tutorId || user.funcionarioId || user.usuarioId;
        }
        this.currentUserSignal.set(user);
      } catch {
        this.logout();
      }
    }
  }

  getCurrentUser() {
    return this.currentUserSignal.asReadonly();
  }

  getCurrentUserValue(): User | null {
    return this.currentUserSignal();
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  isAdmin(): boolean {
    const user = this.currentUserSignal();
    if (!user || !user.cargo) {
      return false;
    }
    const cargo = user.cargo.toLowerCase().trim();
    // Admin é apenas o cargo "Admin" específico
    return cargo === 'admin' || cargo === 'administrador';
  }

  // NOVO: Verificar se é funcionário (veterinário, tosador, atendente)
  isFuncionario(): boolean {
    const user = this.currentUserSignal();
    if (!user || !user.cargo) {
      return false;
    }
    const cargo = user.cargo.toLowerCase().trim();
    const cargosFuncionario = ['veterinário', 'veterinario', 'tosador', 'atendente', 'funcionario'];
    return cargosFuncionario.includes(cargo) || !!user.funcionarioId;
  }

  // NOVO: Verificar se é tutor/cliente
  isTutor(): boolean {
    const user = this.currentUserSignal();
    if (!user) return false;

    const cargo = user.cargo?.toLowerCase().trim();
    return cargo === 'tutor' || !!user.tutorId;
  }

  // Atualizar isUser para ser mais específico
  isUser(): boolean {
    return !!this.currentUserSignal();
  }

  // NOVO: Verificar permissões administrativas (Admin + Funcionários)
  hasAdminAccess(): boolean {
    return this.isAdmin() || this.isFuncionario();
  }

  // NOVO: Recarregar informações do usuário do backend
  reloadUserInfo(): Observable<User> {
    const currentUser = this.currentUserSignal();
    if (!currentUser) {
      throw new Error('Nenhum usuário logado');
    }

    return this.http.get<User>(`${this.apiUrl}/user-info`).pipe(
      tap((user) => {
        this.setCurrentUser({
          ...user,
          id: user.tutorId || user.funcionarioId || user.usuarioId,
        });
      })
    );
  }
}
