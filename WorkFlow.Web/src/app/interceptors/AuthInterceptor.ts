import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Recupera o token salvo durante o login
  const token = localStorage.getItem('token');

  // Se o token existir, clona a requisição e adiciona o Header Authorization
  if (token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(cloned);
  }

  // Se não houver token, segue a requisição original (ex: tela de login)
  return next(req);
};