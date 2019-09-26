import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Subject } from 'rxjs/Subject';

import { LogEntry } from '../shared/log-entry.model';
import { environment } from '../../environments/environment';
import { Pagination } from '../shared/pagination.model';

@Injectable()
export class LogEntryService {
    entriesChanged = new Subject<LogEntry[]>();
    private logEntries: LogEntry[] = [];

    // public pageEntries: any[] = [];
    public pageEntries: Pagination;
    pageEntriesChanged = new Subject<Pagination>();

    api = `${environment.serviceBaseUrl}`;

    constructor(private http: Http) { }

    setLogEntries(logEntries: LogEntry[], pageEntries) {
        this.logEntries = logEntries;
        this.pageEntries = pageEntries;
        this.entriesChanged.next(this.logEntries.slice());
        this.pageEntriesChanged.next(this.pageEntries);
    }

    setEditedBy(id: number, userName: string) {
        const req = this.http.patch(this.api + 'entry/' + id + '/resolved', {
            id: id,
            'EditedBy': userName
        }, {/*withCredentials: true*/})
            .subscribe(
                res => {
                    const index = this.findEntryIndexById(id);
                    this.logEntries[index]['editedBy'] = userName;
                    this.entriesChanged.next(this.logEntries.slice());
                },
                err => {
                    console.error('Error occured', err);
                }
            );
    }

    setDeniedBy(id: number, userName: string) {
        const req = this.http.patch(this.api + 'entry/' + id + '/denied', {
            id: id,
            'DeniedBy': userName
        }, {/*withCredentials: true*/})
            .subscribe(
                res => {
                    const index = this.findEntryIndexById(id);
                    this.logEntries[index]['deniedBy'] = userName;
                    this.entriesChanged.next(this.logEntries.slice());
                },
                err => {
                    console.error('Error occured', err);
                }
            );
    }

    updateNote(id: number, note: string) {
        const req = this.http.patch(this.api + 'entry/' + id + '/note', {
            id: id,
            'note': note
        }, {/*withCredentials: true*/})
            .subscribe(
                res => {
                    const index = this.findEntryIndexById(id);
                    this.logEntries[index]['note'] = note;
                    this.entriesChanged.next(this.logEntries.slice());
                },
                err => {
                    console.error('Error occured', err);
                }
            );
    }

    updateSoftware(id: number, softwareId: number, softwareName: string) {
        const req = this.http.patch(this.api + 'entry/' + id + '/software', {
            id: id,
            'software_ID': softwareId
        })
        .subscribe(
            res => {
                const index = this.findEntryIndexById(id);
                this.logEntries[index]['softwareName'] = softwareName;
                this.entriesChanged.next(this.logEntries.slice());
            },
            err => {
                console.error('Error occured', err);
            }
        );
    }


    getLogEntries() {
        return this.logEntries.slice();
    }

    getLogEntry(id: number) {
        return this.logEntries[id];
    }

    updateLogEntry(index: number, newEntry: LogEntry) {
        this.logEntries[index] = newEntry;
        this.entriesChanged.next(this.logEntries.slice());
    }

    deleteLogEntry(id: number) {
        this.logEntries.splice(this.findEntryIndexById(id), 1);
        this.entriesChanged.next(this.logEntries.slice());
        return this.http.delete(this.api + 'entry/' + id, {/*withCredentials: true*/})
                .toPromise()
                .catch(this.handleError);
    }

    findEntryIndexById(id: number) {
        return this.logEntries.findIndex(d => d.id === id);
    }

    private handleError(error: any): Promise<any> {
        console.error('Some error occured', error);
        return Promise.reject(error.message || error);
    }
}
