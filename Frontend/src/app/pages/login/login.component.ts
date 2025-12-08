import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../service/auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  email: string = '';
  senha: string = '';
  erro: string = '';
  mostrarSenha: boolean = false;
  carregando: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  login(): void {
    this.erro = '';

    // Validações básicas
    if (!this.email || !this.senha) {
      this.erro = 'Preencha todos os campos.';
      return;
    }

    // Validar formato de email
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.email)) {
      this.erro = 'Digite um e-mail válido.';
      return;
    }

    this.carregando = true;

    // Trim apenas início e fim da senha (mantém espaços no meio)
    const senhaTrimmed = this.senha.trim();

    console.log('Tentando login com:', { email: this.email, senha: '***' });

    this.authService.login(this.email, senhaTrimmed).subscribe({
      next: (response) => {
        this.carregando = false;
        console.log('Login bem-sucedido:', response);

        if (this.authService.isAdmin()) {
          this.router.navigate(['/admin']);
        } else {
          this.router.navigate(['/']);
        }
      },
      error: (error: HttpErrorResponse) => {
        this.carregando = false;
        
        console.error('Erro completo de login:', error);
        console.error('Status:', error.status);
        console.error('Mensagem:', error.message);
        console.error('Body:', error.error);
        
        // Mensagens de erro detalhadas
        if (error.status === 401) {
          this.erro = 'E-mail ou senha incorretos. Verifique suas credenciais.';
        } else if (error.status === 400) {
          this.erro = error.error?.message || error.error || 'Dados inválidos. Verifique os campos.';
        } else if (error.status === 0) {
          this.erro = 'Não foi possível conectar ao servidor. Verifique se o backend está rodando na porta 5000.';
        } else if (error.status === 500) {
          this.erro = 'Erro interno do servidor. Verifique os logs do backend.';
        } else {
          this.erro = `Erro ${error.status}: ${error.error?.message || error.message || 'Tente novamente mais tarde.'}`;
        }
      }
    });
  }

  toggleSenha(): void {
    this.mostrarSenha = !this.mostrarSenha;
  }
}
