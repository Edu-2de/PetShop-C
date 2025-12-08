import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Pet } from '../../../model/pet.model';
import { PetService } from '../../../service/pets/pet.service';
import { Tutor } from '../../../model/tutor.model';
import { TutorService } from '../../../service/tutores/tutor.service';
import { AuthService } from '../../../service/auth/auth.service';
import { FormsModule } from '@angular/forms';

interface PetComTutor extends Pet {
  tutor?: Tutor;
}

@Component({
  selector: 'app-pet-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, DatePipe],
  templateUrl: './pet-list.html',
  styleUrls: ['./pet-list.scss'],
})
export class PetListComponent implements OnInit {
  private pets = signal<PetComTutor[]>([]);
  termoBusca = signal<string>('');
  private authService = inject(AuthService);

  petsFiltrados = computed(() => {
    const pets = this.pets();
    const termo = this.termoBusca().toLowerCase();
    if (!termo) {
      return pets;
    }
    return pets.filter(
      (pet) =>
        pet.nome.toLowerCase().includes(termo) ||
        (pet.tutor && pet.tutor.nome.toLowerCase().includes(termo))
    );
  });

  constructor(private petService: PetService, private tutorService: TutorService) {}

  ngOnInit(): void {
    this.carregarPetsComTutores();
  }

  carregarPetsComTutores(): void {
    const user = this.authService.getCurrentUser()();

    console.log('🔍 DEBUG: Usuário logado em pet-list:', user);

    // Se for ADMIN ou FUNCIONÁRIO, mostrar todos os pets
    if (this.authService.isAdmin() || this.authService.isFuncionario()) {
      console.log('👤 Admin/Funcionário: Carregando todos os pets');

      forkJoin({
        pets: this.petService.listar(),
        tutores: this.tutorService.listar(),
      }).subscribe(({ pets, tutores }) => {
        const tutoresMap = new Map(tutores.map((t) => [t.tutorId, t]));
        const petsComTutor: PetComTutor[] = pets.map((pet) => ({
          ...pet,
          tutor: tutoresMap.get(pet.tutorId),
        }));
        this.pets.set(petsComTutor);
        console.log('✅ Todos os pets carregados:', petsComTutor.length);
      });
    }
    // Se for TUTOR, mostrar apenas seus pets
    else if (user?.tutorId) {
      console.log('🐾 Tutor: Carregando apenas meus pets (tutorId:', user.tutorId + ')');

      this.petService.buscarPorTutor(user.tutorId).subscribe({
        next: (pets) => {
          // Carregar dados do tutor para exibir
          const tutorId = user.tutorId;
          if (tutorId) {
            this.tutorService.buscarPorId(tutorId).subscribe({
              next: (tutor) => {
                const petsComTutor: PetComTutor[] = pets.map((pet) => ({
                  ...pet,
                  tutor: tutor,
                }));
                this.pets.set(petsComTutor);
                console.log('✅ Pets do tutor carregados:', petsComTutor.length);
              },
              error: (err) => {
                console.error('❌ Erro ao carregar dados do tutor:', err);
                // Mesmo sem dados do tutor, mostrar os pets
                this.pets.set(pets);
              },
            });
          }
        },
        error: (err) => {
          console.error('❌ Erro ao carregar pets do tutor:', err);
          this.pets.set([]);
        },
      });
    }
    // Se não for nenhum dos casos acima, lista vazia
    else {
      console.log('⚠️ Usuário sem tutorId. Lista de pets vazia.');
      this.pets.set([]);
    }
  }

  buscar(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.termoBusca.set(target.value);
  }

  excluir(id: number | undefined): void {
    if (id === undefined) return;
    if (confirm('Deseja realmente excluir este pet?')) {
      this.petService.deletar(id).subscribe(() => {
        this.pets.update((petsAtuais) => petsAtuais.filter((p) => p.id !== id));
      });
    }
  }
}
