import { Approval } from './approval.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

import { LogEntry } from '../shared/log-entry.model';

import { LogEntryService } from '../log-entry/log-entry.service';
import { PaginatorService } from './paginator.service';
import { AuthenticationService } from './../auth/authentication.service';

import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class DataStorageService {
    private api = `${environment.serviceBaseUrl}`;

    constructor(private http: HttpClient,
                private logEntryService: LogEntryService,
                private paginatorService: PaginatorService) { }

    getLogEntries(page?: number, pageSize?: number, search?: string, role?: string) {
        if (pageSize <= 0 || !pageSize) {
            pageSize = 30;
        }

        if (page < 0 || !page) {
            page = 0;
        }

        let endPoint;

        if (!search) {
            endPoint = `${this.api}entries?page=${page}&pageSize=${pageSize}`;
        } else {
            endPoint = `${this.api}entries/search?page=${page}&pageSize=${pageSize}&username=${search}`;
        }

        if (role === 'consumerRole' && !search) {
            const userName = sessionStorage.getItem('userName').substr(7);
            endPoint = `${this.api}entries/search?page=${page}&pageSize=${pageSize}&username=${userName}`;
        }


        this.http
            .get<LogEntryResponse>(endPoint,
            {
                // withCredentials: true,
                observe: 'response'
            })
            .subscribe(data => {
                // this.data = data.body.result;
                this.logEntryService.setLogEntries(data.body.result, data.body.pagination);
            },
            (err: HttpErrorResponse) => {
                if (err.error instanceof Error) {
                    console.error('A client-side or network error occured.', err.error);
                } else {
                    console.error(`Backend returned code ${err.status}`, err.error);
                }
            });
    }

    get(endPoint: string): any {
        return this.http.get(`${this.api}${endPoint}`,
            {
                observe: 'response'
            }).catch(this.handleError);
    }

    post(endPoint: string, data: any) {
        return this.http.post(this.api + endPoint, data,
            {
            // withCredentials: true,
            observe: 'response'
            })
                .toPromise()
                .catch(this.handleError);
    }

    delete(endpoint: string, id: number) {
        return this.http.delete(`${this.api}${endpoint}/${id}`, {responseType: 'text' })
                .toPromise()
                .catch(this.handleError);
    }

    getUserName() {
        return sessionStorage.getItem('userName');
    }

    getSessionStorageItem(item: string) {
        return sessionStorage.getItem(item);
    }

    public handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for development purposes only
        return Promise.reject(error.message || error);
    }
}


interface LogEntryResponse {
    status: number;
    ok: boolean;
    result: any;
    pagination: any;
}

interface ApprovalEntryResponse {
    status: number;
    ok: boolean;
    body: Approval;
}
