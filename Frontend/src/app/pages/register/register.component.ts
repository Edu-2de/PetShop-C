import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../service/auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';

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

  private authService = inject(AuthService);
  private router = inject(Router);

  /**
   * Valida telefone brasileiro
   * Aceita: (51)98250-6142, 51982506142, (51) 98250-6142, etc
   * Formato: 10 ou 11 dígitos
   */
  validarTelefone(telefone: string): boolean {
    // Remove todos os caracteres não numéricos
    const apenasNumeros = telefone.replace(/\D/g, '');
    
    // Deve ter 10 (fixo) ou 11 (celular) dígitos
    if (apenasNumeros.length < 10 || apenasNumeros.length > 11) {
      return false;
    }

    // Valida DDD (deve estar entre 11 e 99)
    const ddd = parseInt(apenasNumeros.substring(0, 2));
    if (ddd < 11 || ddd > 99) {
      return false;
    }

    return true;
  }

  registrar(form: any) {
    this.erro = '';
    
    // Validações customizadas
    if (!this.form.nome || this.form.nome.length < 3) {
      this.erro = 'Nome deve ter no mínimo 3 caracteres.';
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(this.form.email)) {
      this.erro = 'Digite um e-mail válido.';
      return;
    }

    if (!this.form.senha || this.form.senha.length < 6) {
      this.erro = 'Senha deve ter no mínimo 6 caracteres.';
      return;
    }

    if (!this.validarTelefone(this.form.telefone)) {
      this.erro = 'Telefone inválido. Digite um telefone brasileiro válido com 10 ou 11 dígitos.';
      return;
    }

    if (form.invalid) {
      this.erro = 'Por favor, preencha os campos obrigatórios corretamente.';
      return;
    }

    this.carregando = true;

    // Formatar telefone para envio (apenas números)
    const telefoneFormatado = this.form.telefone.replace(/\D/g, '');

    const novoUsuario = {
      nome: this.form.nome,
      email: this.form.email,
      senha: this.form.senha,
      telefone: telefoneFormatado,
      endereco: this.form.endereco || 'Não informado'
    };

    this.authService.register(novoUsuario).subscribe({
      next: (response) => {
        this.carregando = false;
        alert('Conta criada com sucesso! Você já está logado.');
        this.router.navigate(['/dashboard']);
      },
      error: (err: HttpErrorResponse) => {
        this.carregando = false;
        console.error('Erro ao criar conta:', err);
        
        // Mensagens de erro específicas
        if (err.status === 400) {
          const errorMsg = err.error;
          
          if (typeof errorMsg === 'string') {
            if (errorMsg.toLowerCase().includes('email')) {
              this.erro = 'Este e-mail já está cadastrado. Tente fazer login ou use outro e-mail.';
            } else if (errorMsg.toLowerCase().includes('telefone')) {
              this.erro = 'Este telefone já está cadastrado.';
            } else {
              this.erro = errorMsg;
            }
          } else {
            this.erro = 'Dados inválidos. Verifique os campos e tente novamente.';
          }
        } else if (err.status === 0) {
          this.erro = 'Não foi possível conectar ao servidor. Verifique sua conexão.';
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
