import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../service/auth/auth.service';

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

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {
    this.erro = '';

    if (!this.email || !this.senha) {
      this.erro = 'Por favor, preencha todos os campos.';
      return;
    }

    if (this.authService.login(this.email, this.senha)) {
      const user = this.authService.getCurrentUser()();
      if (user?.role === 'admin') {
        this.router.navigate(['/admin']);
      } else {
        this.router.navigate(['/']);
      }
    } else {
      this.erro = 'Email ou senha incorretos.';
    }
  }

  toggleSenha(): void {
    this.mostrarSenha = !this.mostrarSenha;
  }
}
