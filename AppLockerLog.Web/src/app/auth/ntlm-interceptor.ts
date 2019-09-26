import { Injectable } from '@angular/core';
import {
    HttpRequest,
    HttpHandler,
    HttpInterceptor,
    HttpEvent,
    HttpHeaders
} from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { environment } from '../../environments/environment';
import { RequestOptions } from '@angular/http/src/base_request_options';

@Injectable()
export class NtlmInterceptor implements HttpInterceptor {
    constructor() { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        req = req.clone({
            withCredentials: true,
            setHeaders: {
                'Access-Control-Allow-Origin': environment.origin
            }
        });
        return next.handle(req);
    }
}
