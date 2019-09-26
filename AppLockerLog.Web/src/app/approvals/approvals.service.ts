import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Subject } from 'rxjs/Subject';

import { environment } from '../../environments/environment';

import { Approval, AssignedUserGroup } from './../shared/approval.model';
import { Pagination } from './../shared/pagination.model';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';


@Injectable()
export class ApprovalsService {
    entriesChanged = new Subject<Approval[]>();
    public approvalEntries: Approval[] = [];

    pageEntriesChanged = new Subject<Pagination>();
    public pageEntries: Pagination;

    api = `${environment.serviceBaseUrl}`;

    constructor(private http: HttpClient) { }

    setEntries(entries: Approval[], pageEntries) {
        this.approvalEntries = entries;
        this.pageEntries = pageEntries;
        this.entriesChanged.next(this.approvalEntries.slice());
        this.pageEntriesChanged.next(this.pageEntries);
    }

    getEntries(page?: number, pageSize?: number, search?: string) {
        if (pageSize <= 0 || !pageSize) {
            pageSize = 30;
        }

        if (page < 0 || !page) {
            page = 0;
        }

        let endPoint;

        if (!search) {
            endPoint = `${this.api}approvals?page=${page}&pageSize=${pageSize}`;
        } else {
            endPoint = `${this.api}approvals?page=${page}&pageSize=${pageSize}&query=${search}`;
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

    getEntry(id: number) {
        return this.approvalEntries[id];
    }

    updateNote(id: number, note: string) {
        const req = this.http.patch(this.api + 'approval/' + id + '/note', {
            id: id,
            'reasoning': note
        })
            .subscribe(
                res => {
                    const index = this.findEntryIndexById(id);
                    this.approvalEntries[index]['reasoning'] = note;
                    this.entriesChanged.next(this.approvalEntries.slice());
                },
                err => {
                    console.error('Error occured', err);
                }
            );
    }

    updateComputerName(id: number, computerName: string) {
        const req = this.http.patch(this.api + 'approval/' + id + '/computername', {
            id: id,
            'computerName': computerName
        })
            .subscribe(
                res => {
                    const index = this.findEntryIndexById(id);
                    this.approvalEntries[index]['computerName'] = computerName;
                    this.entriesChanged.next(this.approvalEntries.slice());
                },
                err => {
                    console.error('Error occured', err);
                }
            );
    }

    addGroup(id: number, group: string) {
        let approval = {
            id,
            assignedUserGroups: <any>[]
        };

        let groupArr = [];
        groupArr.push({'group': group});

        approval.id = id;
        approval.assignedUserGroups = groupArr;

        const req = this.http.patch<Approval>(this.api + 'approval/' + id + '/group', approval, {
            observe: 'body'
        })
        .subscribe(
            res => {
                const index = this.findEntryIndexById(id);
                const aug = res.assignedUserGroups;
                const groupId = aug.lastIndexOf(AssignedUserGroup['id']);

                this.approvalEntries[index]['assignedUserGroups'].push({
                    'id': groupId,
                    'approvalId': index,
                    'group': group
                });
                this.entriesChanged.next(this.approvalEntries.slice());
            },
            err => {
                console.error('Error occured', err);
            }
        );
    }

    delete(id: number) {
        this.approvalEntries.splice(this.findEntryIndexById(id), 1);
        this.entriesChanged.next(this.approvalEntries.slice());
        return this.http.delete(this.api + 'approval/' + id, {responseType: 'text'})
                .toPromise()
                .catch(this.handleError);
    }

    deleteGroup(groupId: number, entryId: number) {
        const index = this.findEntryIndexById(entryId);
        const groupIndex = this.approvalEntries[index.valueOf()]
                            .assignedUserGroups
                            .findIndex(d => d.id === groupId);

        this.approvalEntries[index]['assignedUserGroups'].splice(groupIndex, 1);
        this.entriesChanged.next(this.approvalEntries.slice());
        return this.http.delete(this.api + 'approval/' + groupId + '/group', {responseType: 'text'})
                .toPromise()
                .catch(this.handleError);
    }

    findEntryIndexById(id: number) {
        return this.approvalEntries.findIndex(d => d.id === id);
    }

    private handleError(error: any): Promise<any> {
        return Promise.reject(error.message || error);
    }
}

interface Response {
    status: number;
    ok: boolean;
    result: any;
    pagination: any;
}
