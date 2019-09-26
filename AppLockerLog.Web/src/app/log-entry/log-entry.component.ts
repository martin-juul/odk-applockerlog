import { Pagination } from './../shared/pagination.model';
import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { LogEntry } from '../shared/log-entry.model';

import { AuthenticationService } from '../auth/authentication.service';
import { LogEntryService } from '../log-entry/log-entry.service';
import { DataStorageService } from '../shared/data-storage.service';
import { UnsubscriptionError } from 'rxjs/util/UnsubscriptionError';
import { SoftwareService } from '../software/software.service';

@Component({
  selector: 'app-log-entry',
  templateUrl: './log-entry.component.html',
  styleUrls: ['./log-entry.component.css'],
  encapsulation: ViewEncapsulation.None,
  providers: [SoftwareService]
})
export class LogEntryComponent implements OnInit, OnDestroy {
    logEntries: LogEntry[];

    public searchString: string;
    subscription: Subscription;
    paginationSubscription: Subscription;
    availSwSubscription: Subscription;
    paginationPages: number[][] = [];
    pageTotalCount: number;

    path: string[] = ['logEntries'];
    order = -1; // 1 asc, -1 desc

    authorized = this.authService.hasPrivileges();
    readRole = this.authService.readRole;
    consumerRole = this.authService.consumerRole;

  constructor(private logEntryService: LogEntryService,
              private route: ActivatedRoute,
              private router: Router,
              private dataStorageService: DataStorageService,
              private authService: AuthenticationService) { }

    ngOnInit() {
        this.setPage(0);

        this.paginationSubscription = this.logEntryService.pageEntriesChanged
        .subscribe(
            (pageEntries: Pagination) => {
                this.paginationPages = [];
                for (let i = 0; i < pageEntries.totalPages; i++) {
                    this.paginationPages[i] = [i];
                }

                this.pageTotalCount = pageEntries.totalCount;
            }
        );

        this.subscription = this.logEntryService.entriesChanged
        .subscribe(
            (logEntries: LogEntry[]) => {
                this.logEntries = logEntries;
            }
        );

        let searchResult;
        const value = 'microsoft';

        this.availSwSubscription =
            this.dataStorageService.get('software/search?query=' + value)
            .subscribe(data => {
                searchResult = data;
            });

        this.path = ['id'];
        this.order = this.order * (1);

    }

    search(value) {
        this.setPage(0/*, this.searchString*/);
        this.searchString = this.searchString;
    }

    sortTable(prop: string) {
        this.path = prop.split('.');
        this.order = this.order * (-1); // change order
    }

    ngOnDestroy() {
      this.subscription.unsubscribe();
      this.paginationSubscription.unsubscribe();
      this.availSwSubscription.unsubscribe();
    }

    setPage(page: number) {
        if (this.consumerRole) {
            this.dataStorageService.getLogEntries(page, 30, null, 'consumerRole');
        } else if (!this.searchString) {
            this.dataStorageService.getLogEntries(page, 30);
        } else {
            this.dataStorageService.getLogEntries(page, 30, this.searchString);
        }
        scrollTo(0, 0);
    }
}
