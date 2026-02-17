import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef, inject } from '@angular/core'; 
import { Router } from '@angular/router';
import { RequestService } from '../../../services/request-service';
import { MatIcon } from '@angular/material/icon';
import { LoginService } from '../../../services/login-service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, MatIcon],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private requestService = inject(RequestService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);
  private auth = inject(LoginService);

  requests: any[] = [];
  loading: boolean = true;
  isUser: boolean = false;
  errorMessage: string = '';
  paginaAtual: number = 0;
  itensPorPagina: number = 10;
  totalItens: number = 0;

  filtroTexto: string = '';
  filtroStatus: string = '';
  filtroCategoria: string = '';
  filtroPrioridade: string = '';
  readonly Math = Math;

  ngOnInit() {
    this.carregarSolicitacoes();

    this.isUser = this.auth.isUser();
  }


private processarSucesso(res: any) {
  
  this.requests = [...(res?.items || [])]; 
  this.totalItens = res?.totalCount || 0; 

  this.loading = false;
  
  this.cdr.markForCheck(); 
  this.cdr.detectChanges();
}

carregarSolicitacoes() {
  this.loading = true;

  const pagina = {
    termo: this.filtroTexto?.trim() || undefined,
    category: this.filtroCategoria || undefined,
    prioridade: this.filtroPrioridade || undefined,
    status: this.filtroStatus || undefined
  };

  this.requestService.getRequests(pagina.termo,pagina.category,pagina.prioridade,pagina.status, this.paginaAtual)
    .subscribe({
      next: (res: any) => this.processarSucesso(res),
      error: () => { this.loading = false; this.cdr.detectChanges(); }
    });
}

  aplicarFiltros(resetarPagina: boolean = true) {
    if (resetarPagina) this.paginaAtual = 0;
    this.carregarSolicitacoes(); 
  }

  aoBuscar(event: any) {
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

  mudarPagina(novaPagina: number) {
    this.paginaAtual = novaPagina;
    this.aplicarFiltros(false);
  }

  getStatusLabel(status: number): string {
    switch(status){
    case 0: return 'Pendente';
    case 1: return 'Aprovado';
    case 2: return 'Rejeitado'
    default: return 'Desconhecido'
    }
  }

  getPrioridadeLabel(priority: number): string {
    switch(priority){
    case 0: return 'BAIXA';
    case 1: return 'MÉDIA';
    case 2: return 'ALTA';
    default: return 'Desconhecido';
    }
 
  }

  getCategoryLabel(category: number): string {
      switch(category){
      case 1: return 'COMPRAS';
      case 2: return 'TI';
      case 3: return 'REEMBOLSO';
      case 4: return 'OUTROS';
      default: return'Desconhecido';
    }

  }

  adicionarNova() {
    if(this.isUser){
    this.router.navigate(['/new-request']);
    }
  }

  verDetalhe(id: string) { 
    this.router.navigate(['/request-detail', id]);
  }

  logOut(){
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}