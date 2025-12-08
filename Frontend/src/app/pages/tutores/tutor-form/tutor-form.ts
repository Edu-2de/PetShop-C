import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Tutor } from '../../../model/tutor.model';
import { TutorService } from '../../../service/tutores/tutor.service';

@Component({
  selector: 'app-tutor-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './tutor-form.html',
  styleUrl: './tutor-form.scss'
})
export class TutorFormComponent implements OnInit {
  tutor: Partial<Tutor> = { nome: '', email: '', telefone: '', endereco: '' };
  isEdit: boolean = false;

  constructor(
    private tutorService: TutorService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.tutorService.buscarPorId(Number(id)).subscribe((tutor) => {
        this.tutor = tutor;
      });
    }
  }

  salvar(): void {
    if (this.isEdit && this.tutor.tutorId) {
      this.tutorService.atualizar(this.tutor.tutorId, this.tutor as Tutor).subscribe(() => {
        this.router.navigate(['/tutores']);
      });
    } else {
      this.tutorService.criar(this.tutor as Tutor).subscribe(() => {
        this.router.navigate(['/tutores']);
      });
    }
  }
}
