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
  carregando: boolean = false; // Novo estado

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  login(): void {
    this.erro = '';
    if (!this.email || !this.senha) {
      this.erro = 'Preencha todos os campos.';
      return;
    }

    this.carregando = true;

    // Agora usamos subscribe no Observable real
    this.authService.login(this.email, this.senha).subscribe({
      next: () => {
        this.carregando = false;

        // A verificação agora usa a nova lista de cargos
        if (this.authService.isAdmin()) {
          this.router.navigate(['/admin']);
        } else {
          this.router.navigate(['/']); // Clientes vão para home
        }
      },
    });
  }

  toggleSenha(): void {
    this.mostrarSenha = !this.mostrarSenha;
  }
}
