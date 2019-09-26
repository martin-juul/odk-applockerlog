import { Component, OnInit, ViewEncapsulation, Input, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs/Subject';

import { LogEntry } from '../../shared/log-entry.model';
import { Software } from '../../software/software-model';

import { LogEntryService } from '../log-entry.service';
import { DataStorageService } from '../../shared/data-storage.service';
import { AuthenticationService } from '../../auth/authentication.service';
import { SoftwareService } from './../../software/software.service';

@Component({
  selector: '[app-log-entry-item]',
  templateUrl: './log-entry-item.component.html',
  styleUrls: ['./log-entry-item.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class LogEntryItemComponent implements OnInit, OnDestroy {
    @Input() logEntry: LogEntry;
    @Input() index: number;
    @Input() softwareEntries: Software;
    writeRole = this.authService.writeRole;
    readRole = this.authService.readRole;
    consumerRole = this.authService.consumerRole;
    menuActive = false;

    constructor(private logEntryService: LogEntryService,
                private dataStorageService: DataStorageService,
                private authService: AuthenticationService,
                private softwareService: SoftwareService) { }

    ngOnInit() {
    }

    updateEditedBy(id: number, mode: string = 'approve'): void {
        if (mode === 'approve') {
            // Approve
            this.logEntryService.setEditedBy(id, this.dataStorageService.getUserName());
        } else {
            // Unapprove
            this.logEntryService.setEditedBy(id, null);
        }
    }

    updateDeniedBy(id: number, mode: string = 'deny'): void {
        if (mode === 'deny') {
            // Deny
            this.logEntryService.setDeniedBy(id, this.dataStorageService.getUserName());
        } else {
            // Undeny
            this.logEntryService.setDeniedBy(id, null);
        }
    }

    deleteEntry(id: number): void {
        if (confirm('Are you sure you want to delete the request with id: ' + id)) {
            this.logEntryService.deleteLogEntry(id);
        }
    }

    /* updateSoftware(id: number, value: string): void {
        this.dataStorageService.get('api/software/search?pageSize=100&page=0');
    } */

    searchSoftware(id: number, value?: string) {
        let searchResult: string;
        let query;

        if (value) {
            query = '?query=' + value;
        } else {
            query = null;
        }

        const search = this.dataStorageService
            .get('software/search' + query)
            .subscribe(data => {
                searchResult = data;
            });

    }

    captureChange(id: number, value: string, field: string): void {
        setTimeout(() => {
            if (field === 'note') {
                this.logEntryService.updateNote(id, value);
            } else {
                console.error('undefined field.');
            }
        }, 500);
    }

    isMenuActive() {
        return !this.menuActive;
    }

    menuStyles() {
        console.log('style triggered!');
        const styles = {
            'z-index': this.isMenuActive() ? '10' : '5',
        };

        return styles;
    }

    setMenuStyle(event: any) {
        console.log(event);
    }

    ngOnDestroy() {
    }

}
