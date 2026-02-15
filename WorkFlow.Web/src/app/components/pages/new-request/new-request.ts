import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { RequestService } from '../../../services/request-service';
import { MatIcon } from "@angular/material/icon";

@Component({
  selector: 'app-new-request',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatIcon],
  templateUrl: './new-request.html',
  styleUrl: './new-request.css'
})
export class NewRequest implements OnInit {
  requestForm: FormGroup;
  requestId: string | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private requestService: RequestService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.requestForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(5)]],
      description: ['', [Validators.required]],
      priority: [0, Validators.required],
      category: [1, Validators.required],
      status: [0]
    });
  }

  ngOnInit() {
    this.requestId = this.route.snapshot.paramMap.get('id');
    if (this.requestId) {
      this.carregarDadosParaEdicao(this.requestId);
    }
  }

  carregarDadosParaEdicao(id: string) {
    this.loading = true;
    this.requestService.getById(id).subscribe({
      next: (data) => {
        this.requestForm.patchValue(data);
        this.loading = false;
      },
      error: () => {
        alert('Erro ao carregar dados para edição.');
        this.router.navigate(['/home']);
      }
    });
  }

  salvar() {
  if (this.requestForm.invalid) return;

  this.loading = true;
  
  const dadosBrutos = this.requestForm.value;
  const payload = {
    ...dadosBrutos,
    priority: Number(dadosBrutos.priority),
    category: Number(dadosBrutos.category),
    status: Number(dadosBrutos.status)
  };

  if (this.requestId) {
    this.loading = false;
  } else {
    this.requestService.create(payload).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/home']);
      },
      error: (err) => {
        this.loading = false;
        console.error('Erro detalhado da API:', err.error?.errors);
        alert('Erro ao salvar: Verifique os dados enviados.');
      }
    });
  }
}

  cancelar() {
    this.router.navigate(['/home']);
  }
}