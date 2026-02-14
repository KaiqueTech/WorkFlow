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

  constructor(
    private requestService: RequestService, 
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    console.log('Componente Home iniciado');
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

  editar(id: string) {
    this.router.navigate(['/new-request', id]);
  }

  remover(id: string) {
    if (confirm('Deseja realmente remover esta solicitação?')) {
      this.requestService.delete(id).subscribe({
        next: () => {
          alert('Removido com sucesso!');
          this.carregarSolicitacoes();
        },
        error: () => alert('Erro ao remover solicitação.')
      });
    }
  }

  logOut(){
    localStorage.removeItem('token');

    localStorage.clear();

    this.router.navigate(['/login']);
  }
}