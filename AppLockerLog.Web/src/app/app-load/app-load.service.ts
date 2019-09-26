import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { AuthenticationService } from '../auth/authentication.service';
import { User } from '../shared/user.model';
import { environment } from '../../environments/environment';

@Injectable()
export class AppLoadService {
    constructor(private http: HttpClient,
                private authService: AuthenticationService) { }

    authenticate(user: User) {
        this.authService.checkRolesAndName(user);
    }

    getUser(): Promise<any> {
        const promise = this.http.get(`${environment.serviceBaseUrl}auth/getUser`)
            .toPromise()
            .then((user: User) => {
                this.authenticate(user);
            });

        return promise;
    }
}
