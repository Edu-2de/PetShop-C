import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';

export interface User {
  id: number;
  nome: string;
  email: string;
  role: 'admin' | 'user';
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUser = signal<User | null>(null);
  private readonly STORAGE_KEY = 'sigapet_user';

  constructor(private router: Router) {
    // Recuperar usuário do localStorage ao inicializar
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    const userJson = localStorage.getItem(this.STORAGE_KEY);
    if (userJson) {
      try {
        const user = JSON.parse(userJson);
        this.currentUser.set(user);
      } catch {
        localStorage.removeItem(this.STORAGE_KEY);
      }
    }
  }

  login(email: string, senha: string): boolean {
    // Simulação de login - TEMPORÁRIO
    // Em produção, isso faria uma chamada HTTP para o backend
    
    // Admin: admin@sigapet.com / admin123
    // User: user@sigapet.com / user123
    
    if (email === 'admin@sigapet.com' && senha === 'admin123') {
      const user: User = {
        id: 1,
        nome: 'Administrador',
        email: 'admin@sigapet.com',
        role: 'admin'
      };
      this.setCurrentUser(user);
      return true;
    }
    
    if (email === 'user@sigapet.com' && senha === 'user123') {
      const user: User = {
        id: 2,
        nome: 'Usuário Comum',
        email: 'user@sigapet.com',
        role: 'user'
      };
      this.setCurrentUser(user);
      return true;
    }
    
    return false;
  }

  logout(): void {
    this.currentUser.set(null);
    localStorage.removeItem(this.STORAGE_KEY);
    this.router.navigate(['/']);
  }

  private setCurrentUser(user: User): void {
    this.currentUser.set(user);
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(user));
  }

  getCurrentUser() {
    return this.currentUser.asReadonly();
  }

  isAuthenticated(): boolean {
    return this.currentUser() !== null;
  }

  isAdmin(): boolean {
    return this.currentUser()?.role === 'admin';
  }

  isUser(): boolean {
    return this.currentUser()?.role === 'user';
  }
}
