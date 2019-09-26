import { Observable } from 'rxjs/Observable';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Injectable } from '@angular/core';

import { AuthenticationService } from './authentication.service';
import { map } from 'rxjs/operator/map';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private authService: AuthenticationService, private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
        let url = state.url;
        return this.checkLogin(url);
    }

    checkLogin(url: string): boolean {
        if (this.authService.hasPrivileges()) {
            return true;
        } else {
            console.error('Not allowed');
            return false;
        }
    }
}
