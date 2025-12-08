import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Pet } from '../../../model/pet.model';
import { PetService } from '../../../service/pets/pet.service';
import { Tutor } from '../../../model/tutor.model';
import { TutorService } from '../../../service/tutores/tutor.service';
import { AuthService } from '../../../service/auth/auth.service';

@Component({
  selector: 'app-pet-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './pet-form.html',
  styleUrls: ['./pet-form.scss']
})
export class PetFormComponent implements OnInit {
  pet: Partial<Pet> = { 
    nome: '', 
    especie: 'Cão', 
    raca: '', 
    dataNascimento: new Date(), 
    sexo: 'Macho',
    tutorId: 0
  };
  
  tutores: Tutor[] = [];
  isEdit = false;
  erro = '';
  isAdmin = false;

  private petService = inject(PetService);
  private tutorService = inject(TutorService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  ngOnInit(): void {
    // Verificar se é admin para mostrar seletor de tutores
    this.isAdmin = this.authService.isAdmin();
    
    if (this.isAdmin) {
      // Admin pode selecionar qualquer tutor
      this.tutorService.listar().subscribe(tutores => this.tutores = tutores);
    } else {
      // Cliente comum - usar o próprio tutorId
      const user = this.authService.getCurrentUserValue();
      if (user?.tutorId) {
        this.pet.tutorId = user.tutorId;
      } else {
        this.erro = 'Erro: Usuário não está vinculado a um tutor válido.';
        return;
      }
    }

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.petService.buscarPorId(Number(id)).subscribe({
        next: (pet) => {
          this.pet = pet;
        },
        error: (err) => {
          this.erro = 'Erro ao carregar pet para edição.';
          console.error(err);
        }
      });
    }
  }

  salvar(): void {
    this.erro = '';

    // Validações
    if (!this.pet.nome || this.pet.nome.trim().length < 2) {
      this.erro = 'Nome do pet deve ter pelo menos 2 caracteres.';
      return;
    }

    if (!this.pet.tutorId || this.pet.tutorId === 0) {
      this.erro = 'TutorId inválido. Entre em contato com o suporte.';
      return;
    }

    if (this.isEdit && this.pet.animalId) {
      this.petService.atualizar(this.pet.animalId, this.pet).subscribe({
        next: () => {
          alert('Pet atualizado com sucesso!');
          this.router.navigate(['/pets']);
        },
        error: (err) => {
          this.erro = 'Erro ao atualizar pet. Verifique os dados.';
          console.error('Erro na atualização:', err);
        }
      });
    } else {
      this.petService.criar(this.pet).subscribe({
        next: () => {
          alert('Pet cadastrado com sucesso!');
          this.router.navigate(['/pets']);
        },
        error: (err) => {
          this.erro = 'Erro ao cadastrar pet. Verifique os dados.';
          console.error('Erro na criação:', err);
        }
      });
    }
  }
}
