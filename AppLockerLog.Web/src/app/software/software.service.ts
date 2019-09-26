import { HttpErrorResponse, HttpClient, HttpHeaders } from '@angular/common/http';
import { Subject } from 'rxjs/Subject';
import { Injectable } from '@angular/core';

import { DataStorageService } from './../shared/data-storage.service';

import { Software } from './software-model';
import { Pagination } from '../shared/pagination.model';
import { environment } from '../../environments/environment';

@Injectable()
export class SoftwareService {
    entriesChanged = new Subject<Software[]>();
    private softwareEntries: Software[] = [];

    pageEntriesChanged = new Subject<Pagination>();
    public pageEntries: Pagination;

    api = `${environment.serviceBaseUrl}`;

    constructor(private http: HttpClient,
                private dataStorageService: DataStorageService) { }

    setEntries(entries: Software[], pageEntries) {
        this.softwareEntries = entries;
        this.pageEntries = pageEntries;
        this.entriesChanged.next(this.softwareEntries.slice());
        this.pageEntriesChanged.next(this.pageEntries);
    }

    getEntries(page?: number, pageSize?: number, state?: string, search?: string) {
        if (pageSize <= 0 || !pageSize) {
            pageSize = 30;
        }

        if (page < 0 || !page) {
            page = 0;
        }

        let endPoint;

        if (!search && state) {
            endPoint = `${this.api}software?page=${page}&pageSize=${pageSize}&state=${state}`;
        } else if (!search && !state) {
            endPoint = `${this.api}software?page=${page}&pageSize=${pageSize}`;
        } else {
            endPoint = `${this.api}software?page=${page}&pageSize=${pageSize}&state=${state}&query=${search}`;
        }


        this.http
            .get<Response>(endPoint, {
                observe: 'response'
            })
            .subscribe(data => {
                this.setEntries(data.body.result, data.body.pagination);
            },
            (err: HttpErrorResponse) => {
                if (err.error instanceof Error) {
                    console.error('A client-side or network error occured.', err.error);
                } else {
                    console.error(`Backend returned code ${err.status}`, err.error);
                }
            });
    }

    updateReasoning(id: number, reasoning: string) {
        const req = this.http.patch(`${this.api}software/${id}/reasoning`, {
            id: id,
            'reasoning': reasoning
        }).subscribe(
            res => {
                const index = this.findEntryIndexById(id);
                this.softwareEntries[index]['reasoning'] = reasoning;
                this.entriesChanged.next(this.softwareEntries.slice());
            },
            err => {
                console.error('Error occured', err);
            }
        );
    }

    public handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for development purposes only
        return Promise.reject(error.message || error);
    }

    getEntry(id: number) {
        return this.softwareEntries[id];
    }

    deleteEntry(id: number) {
        this.softwareEntries.splice(this.findEntryIndexById(id), 1);
        this.entriesChanged.next(this.softwareEntries.slice());
        return this.dataStorageService.delete(`software`, id);
    }

    findEntryIndexById(id: number) {
        return this.softwareEntries.findIndex(d => d.id === id);
    }
}

interface Response {
    status: number;
    ok: boolean;
    result: any;
    pagination: any;
}
