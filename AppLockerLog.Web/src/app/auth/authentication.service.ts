import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { DataStorageService } from '../shared/data-storage.service';

import { environment } from '../../environments/environment';
import { Role } from '../shared/role.enum';

@Injectable()
export class AuthenticationService {
    constructor(private http: HttpClient,
                private dataStorageService: DataStorageService) { }
    Authenticated = false;
    writeRole = false;
    readRole = false;
    consumerRole = false;
    username: string;

    checkRolesAndName(user): void {
        sessionStorage.setItem('userName', user['userName']);
        this.username = user['userName'];
        if (user[Role.incidentResolverGroup] && user[Role.incidentReaderGroup]) {
            this.Authenticated = true;
            this.writeRole = true;
            this.readRole = true;
            // this.consumerRole = true; /* TODO: Remove */
        } else if (user[Role.incidentResolverGroup]) {
            this.Authenticated = true;
            this.writeRole = true;
            this.readRole = true;
        } else if (user[Role.incidentReaderGroup]) {
            this.Authenticated = true;
            this.readRole = true;
        } else if (user[user[Role.userReaderGroup]]) {
            this.Authenticated = true;
            this.consumerRole = true;
        }
    }

    checkWritePermission(): boolean {
        if (this.writeRole) {
            return true;
        }
        return false;
    }


    isAuthenticated() {
        return this.Authenticated !== false;
    }

    hasPrivileges(): Observable<boolean> | boolean {
        if (this.isAuthenticated()) {
            if (this.readRole || this.consumerRole || this.writeRole) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
}
