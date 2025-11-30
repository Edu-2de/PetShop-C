import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { TutorService } from '../../service/tutores/tutor.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  // Modelo de dados para o formulário
  form = {
    nome: '',
    email: '',
    senha: '',
    telefone: '',
    endereco: '' // Opcional no início
  };

  erro: string = '';
  carregando: boolean = false;
  mostrarSenha: boolean = false;

  private tutorService = inject(TutorService);
  private router = inject(Router);

  registrar() {
    this.erro = '';
    
    if (!this.form.nome || !this.form.email || !this.form.senha) {
      this.erro = 'Por favor, preencha os campos obrigatórios.';
      return;
    }

    this.carregando = true;

    // Enviamos 'any' porque a interface Tutor padrão não tem 'senha', mas o DTO do backend espera.
    const novoUsuario: any = {
      nome: this.form.nome,
      email: this.form.email,
      senha: this.form.senha,
      telefone: this.form.telefone,
      endereco: this.form.endereco
    };

    this.tutorService.criar(novoUsuario).subscribe({
      next: () => {
        this.carregando = false;
        alert('Conta criada com sucesso! Faça login para continuar.');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.carregando = false;
        console.error(err);
        if (err.error && typeof err.error === 'string') {
          this.erro = err.error; // Ex: "Email já em uso"
        } else {
          this.erro = 'Erro ao criar conta. Tente novamente mais tarde.';
        }
      }
    });
  }

  toggleSenha() {
    this.mostrarSenha = !this.mostrarSenha;
  }
}
