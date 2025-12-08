import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { AdminDashboardComponent } from './pages/admin/admin-dashboard.component';
import { ServicoPetListComponent } from './pages/servicos-pet/servicos-pet';
import { ServicoFormComponent } from './pages/servicos-pet/servicos-pet.form/servicos-pet.form';
import { ProdutoListComponent } from './pages/produtos/produto-list';
import { ProdutoFormComponent } from './pages/produtos/produto-form/produto-form';
import { ProdutoDetailComponent } from './pages/produtos/produto-detail/produto-detail';
import { CategoriaListComponent } from './pages/categorias/categoria-list/categoria-list';
import { CategoriaFormComponent } from './pages/categorias/categoria-form/categoria-form';
import { ForncedorListComponent } from './pages/forncedor/forncedor-list/forncedor-list';
import { ForncedorFormComponent } from './pages/forncedor/forncedor-form/forncedor-form';
import { FuncionarioListComponent } from './pages/funcionarios/funcionario-list/funcionario-list';
import { FuncionarioFormComponent } from './pages/funcionarios/funcionario-form/funcionario-form';
import { TutorListComponent } from './pages/tutores/tutor-list/tutor-list';
import { TutorFormComponent } from './pages/tutores/tutor-form/tutor-form';
import { PetListComponent } from './pages/pets/pet-list/pet-list';
import { PetFormComponent } from './pages/pets/pet-form/pet-form';
import { AgendaListComponent } from './pages/agenda-list/agenda-list';
import { AgendaFormComponent } from './pages/agenda-form/agenda-form';
import { MinhasComprasComponent } from './pages/minhas-compras/minhas-compras.component';
import { PerfilComponent } from './pages/perfil/perfil.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'login', component: LoginComponent },
  { path: 'cadastrar', component: RegisterComponent },
  { path: 'admin', component: AdminDashboardComponent, canActivate: [authGuard] },

  // Serviços
  { path: 'servicos', component: ServicoPetListComponent, canActivate: [authGuard] },
  { path: 'servicos/novo', component: ServicoFormComponent, canActivate: [authGuard] },
  { path: 'servicos/editar/:id', component: ServicoFormComponent, canActivate: [authGuard] },

  // Produtos - CORRIGIDO: /produtos/:id ao invés de /produtos/detalhes/:id
  { path: 'produtos', component: ProdutoListComponent },
  { path: 'produtos/novo', component: ProdutoFormComponent, canActivate: [authGuard] },
  { path: 'produtos/editar/:id', component: ProdutoFormComponent, canActivate: [authGuard] },
  { path: 'produtos/:id', component: ProdutoDetailComponent }, // ROTA CORRIGIDA

  // Categorias
  { path: 'categorias', component: CategoriaListComponent, canActivate: [authGuard] },
  { path: 'categorias/novo', component: CategoriaFormComponent, canActivate: [authGuard] },
  { path: 'categorias/editar/:id', component: CategoriaFormComponent, canActivate: [authGuard] },

  // Fornecedores
  { path: 'fornecedores', component: ForncedorListComponent, canActivate: [authGuard] },
  { path: 'fornecedores/novo', component: ForncedorFormComponent, canActivate: [authGuard] },
  { path: 'fornecedores/editar/:id', component: ForncedorFormComponent, canActivate: [authGuard] },

  // Funcionários
  { path: 'funcionarios', component: FuncionarioListComponent, canActivate: [authGuard] },
  { path: 'funcionarios/novo', component: FuncionarioFormComponent, canActivate: [authGuard] },
  { path: 'funcionarios/editar/:id', component: FuncionarioFormComponent, canActivate: [authGuard] },

  // Tutores
  { path: 'tutores', component: TutorListComponent, canActivate: [authGuard] },
  { path: 'tutores/novo', component: TutorFormComponent, canActivate: [authGuard] },
  { path: 'tutores/editar/:id', component: TutorFormComponent, canActivate: [authGuard] },

  // Pets
  { path: 'pets', component: PetListComponent, canActivate: [authGuard] },
  { path: 'pets/novo', component: PetFormComponent, canActivate: [authGuard] },
  { path: 'pets/editar/:id', component: PetFormComponent, canActivate: [authGuard] },

  // Agenda
  { path: 'agenda', component: AgendaListComponent, canActivate: [authGuard] },
  { path: 'agenda/novo', component: AgendaFormComponent, canActivate: [authGuard] },
  { path: 'agenda/editar/:id', component: AgendaFormComponent, canActivate: [authGuard] },

  // Cliente
  { path: 'vendas/minhas', component: MinhasComprasComponent, canActivate: [authGuard] },
  { path: 'perfil', component: PerfilComponent, canActivate: [authGuard] },

  // Redirecionamento padrão
  { path: '**', redirectTo: '' }
];
