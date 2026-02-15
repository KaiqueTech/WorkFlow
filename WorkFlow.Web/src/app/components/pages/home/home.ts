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

  filtroTexto: string = '';
  filtroStatus: string = '';
  filtroCategoria: string = '';
  filtroPrioridade: string = '';
  readonly Math = Math;


  constructor(
    private requestService: RequestService, 
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    console.log("home carregou aqui");
    this.carregarSolicitacoes();
  }


private processarSucesso(res: any) {
  console.log('1. Chegou no processarSucesso:', res);
  
  this.requests = [...(res?.items || [])]; 
  this.totalItens = res?.totalCount || 0; 

  this.loading = false;
  
  this.cdr.markForCheck(); 
  this.cdr.detectChanges();
  
  console.log('2. Variáveis atualizadas:', this.requests, this.totalItens);
}

carregarSolicitacoes() {
  this.loading = true;

  const p = {
    t: this.filtroTexto?.trim() || undefined,
    c: this.filtroCategoria || undefined,
    pr: this.filtroPrioridade || undefined,
    s: this.filtroStatus || undefined
  };

  this.requestService.getRequests(p.t, p.c, p.pr, p.s, this.paginaAtual)
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

  getStatusLabel(status: any): string {
    const s = Number(status);
    if (s === 0) return 'Pendente';
    if (s === 1) return 'Aprovado';
    if (s === 2) return 'Rejeitado';
    return 'Desconhecido';
  }

  getPrioridadeLabel(priority: any): string {
    const p = Number(priority);
    if (p === 0) return 'BAIXA';
    if (p === 1) return 'MÉDIA';
    if (p === 2) return 'ALTA';
    return 'Desconhecido';
  }

  getCategoryLabel(category: any): string {
    const c = Number(category);
    if (c === 1) return 'COMPRAS';
    if (c === 2) return 'TI';
    if (c === 3) return 'REEMBOLSO';
    if (c === 4) return 'OUTROS';
    return 'Desconhecido';
  }

  adicionarNova() {
    this.router.navigate(['/new-request']);
  }

  verDetalhe(id: string) { 
    this.router.navigate(['/request-detail', id]);
  }

  logOut(){
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}