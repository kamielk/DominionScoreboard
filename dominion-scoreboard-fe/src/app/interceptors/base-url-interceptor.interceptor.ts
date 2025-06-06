import { environment } from '../../../environments/environment'
import { HttpInterceptorFn } from '@angular/common/http';

export const baseUrlInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const apiBase = environment.apiUrl;

  const request = req.clone({ url: `${apiBase}${req.url}` });
  return next(request);
};
