import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RequestService } from '../../../services/request-service';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { LoginService } from '../../../services/login-service';

@Component({
  selector: 'app-request-detail',
  standalone: true,
  templateUrl: './request-detail.html',
  styleUrl: './request-detail.css',
  imports: [CommonModule, MatIconModule]
})
export class RequestDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private requestService = inject(RequestService);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);
  private auth = inject(LoginService)

  request: any;
  history: any[] = [];
  requestId: string | null = null;
  loading: boolean = true;
  errorMessage: string = '';
  
  isManager: boolean = false;

  readonly statusConfig: any = {
    0: { label: 'Pendente', color: 'text-yellow-500', bg: 'bg-yellow-500/10', icon: 'schedule' },
    1: { label: 'Aprovado', color: 'text-green-500', bg: 'bg-green-500/10', icon: 'check_circle' },
    2: { label: 'Rejeitado', color: 'text-red-500', bg: 'bg-red-500/10', icon: 'cancel' }
  };

  readonly priorityConfig: any = {
    0: { label: 'Baixa', color: 'text-blue-400', icon: 'low_priority' },
    1: { label: 'Média', color: 'text-yellow-400', icon: 'reorder' },
    2: { label: 'Alta', color: 'text-red-400', icon: 'priority_high' }
  };

  readonly categoryMap: Record<number, string> = {
    1: 'Compras', 2: 'TI', 3: 'REEMBOLSO', 4: 'OUTROS'
  };

  ngOnInit() {
  this.requestId = this.route.snapshot.paramMap.get('id');
    
    this.isManager = this.auth.isManager();

    this.carregarDados();
}

  carregarDados() {
    this.loading = true;
    this.errorMessage = '';

    this.requestService.getById(this.requestId!).subscribe({
      next: (res: any) => {
        if (res) {
          this.request = {
            ...res,
            status: res.status !== undefined ? Number(res.status) : 0,
            priority: res.priority !== undefined ? Number(res.priority) : 0,
            category: res.category !== undefined ? Number(res.category) : 1
          };
        }
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.errorMessage = "Erro ao carregar detalhes.";
        this.loading = false;
        this.cdr.detectChanges();
      }
    });

    this.requestService.getHistory(this.requestId!).subscribe({
      next: (res: any) => {
        const rawHistory = res.items || (Array.isArray(res) ? res : []);
        this.history = rawHistory.map((h: any) => ({
          ...h,
          toStatus: h.toStatus !== undefined ? Number(h.toStatus) : h.toStatus
        }));
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.history = [];
        this.cdr.detectChanges();
      }
    });
  }

  abrirModalDecisao(isApprove: boolean) {
    if (!this.isManager) {
      alert('Apenas gestores podem aprovar ou rejeitar solicitações.');
      return;
    }

    const acao = isApprove ? 'Aprovar' : 'Rejeitar';
    const comentario = prompt(`Deseja ${acao} esta solicitação? Adicione um comentário:`);

    if (comentario === null) return;

    if (!isApprove && (!comentario || comentario.trim() === '')) {
      alert('É obrigatório informar um motivo para a rejeição.');
      return;
    }

    const acaoObservable = isApprove 
      ? this.requestService.approveRequest(this.requestId!, comentario) 
      : this.requestService.rejectRequest(this.requestId!, comentario);

    acaoObservable.subscribe({
      next: () => {
        alert(`Solicitação ${isApprove ? 'aprovada' : 'rejeitada'} com sucesso!`);
        this.carregarDados();
      },
      error: () => alert('Erro ao processar a decisão.')
    });
  }

  voltar() {
    this.router.navigate(['/home']);
  }
}