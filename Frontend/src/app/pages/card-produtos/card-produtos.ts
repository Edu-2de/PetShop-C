import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-card-produtos',
  standalone: true,
  imports: [CommonModule, RouterModule], // Importando módulos básicos
  templateUrl: './card-produtos.html',
  styleUrl: './card-produtos.scss'
})
export class CardProdutos {

}
