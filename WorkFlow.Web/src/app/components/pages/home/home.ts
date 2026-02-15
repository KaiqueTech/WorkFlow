import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core'; 
import { Router } from '@angular/router';
import { RequestService } from '../../../services/request-service';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, MatIcon],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  requests: any[] = [];
  loading: boolean = true;
  errorMessage: string = '';
  paginaAtual: number = 0;
  itensPorPagina: number = 10;
  totalItens: number = 0;

  constructor(
    private requestService: RequestService, 
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.carregarSolicitacoes();
  }

  carregarSolicitacoes() {
    this.loading = true;
    this.errorMessage = '';

    this.requestService.getRequests().subscribe({
      next: (res: any) => {
        
        this.requests = res.items || (Array.isArray(res) ? res : []);
        
        this.loading = false;
        
        this.cdr.detectChanges(); 
        
      },
      error: (err) => {
        this.errorMessage = "Erro na comunicação com a API.";
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

aoBuscar(event: any) {
  const termo = event.target.value;
  
  this.requestService.getRequests(termo).subscribe({
    next: (res: any) => {
      this.requests = res.items || [];
      this.cdr.detectChanges();
    }
  });
}

filtroTexto: string = '';
filtroStatus: string = '';
filtroCategoria: string = '';
filtroPrioridade: string = '';

aplicarFiltros(resetarPagina: boolean = true) {
  if(resetarPagina) this.paginaAtual = 0;
  this.loading = true;
  this.requestService.getRequests(
    this.filtroTexto, 
    this.filtroCategoria, 
    this.filtroPrioridade,
    this.filtroStatus,
    this.paginaAtual
  ).subscribe({
    next: (res: any) => {
      this.requests = res.items || [];
      this.loading = false;
      this.cdr.detectChanges();
    }
  });
}

mudarPagina(novaPagina: number) {
  console.log("indo para a pagina: ", novaPagina)
  this.paginaAtual = novaPagina;
  this.aplicarFiltros(false);
}

aoMudarTexto(event: any) {
  this.filtroTexto = event.target.value;
  this.aplicarFiltros();
}

aoMudarStatus(event: any){
  this.filtroStatus = event.target.value;
  this.aplicarFiltros();
}

aoMudarCategoria(event: any) {
  this.filtroCategoria = event.target.value;
  this.aplicarFiltros();
}

aoMudarPrioridade(event: any) {
  this.filtroPrioridade = event.target.value;
  this.aplicarFiltros();
}

getStatusLabel(status: number): string {
  switch (status) {
    case 0: return 'Pendente';
    case 1: return 'Aprovado';
    case 2: return 'Rejeitado';
    default: return 'Desconhecido';
  }
}

getPrioridadeLabel(priority: number): string {
  switch (priority) {
    case 0: return 'BAIXA';
    case 1: return 'MÉDIA';
    case 2: return 'ALTA';
    default: return 'Desconhecido';
  }
}

getCategoryLabel(category: number): string {
  switch (category) {
    case 1: return 'COMPRAS';
    case 2: return 'TI';
    case 3: return 'REEMBOLSO';
    case 4: return 'OUTROS';
    default: return 'Desconhecido';
  }
}

  adicionarNova() {
    this.router.navigate(['/new-request']);
  }

  verDetalhe(id: string) { 
    this.router.navigate(['/request-detail', id]);
  }

  logOut(){
    localStorage.removeItem('token');

    localStorage.clear();

    this.router.navigate(['/login']);
  }
}