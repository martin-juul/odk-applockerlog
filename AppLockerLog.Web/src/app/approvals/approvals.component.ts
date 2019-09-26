import { Pagination } from './../shared/pagination.model';
import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';

import { DataStorageService } from './../shared/data-storage.service';
import { ApprovalsService } from './approvals.service';
import { AuthenticationService } from '../auth/authentication.service';

import { Approval } from './../shared/approval.model';

@Component({
  selector: 'app-approvals',
  templateUrl: './approvals.component.html',
  styleUrls: ['./approvals.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ApprovalsComponent implements OnInit, OnDestroy {
  approvalEntries: Approval[];
  subscription: Subscription;
  paginationSubscription: Subscription;
  writeRole = this.authService.writeRole;
  readRole = this.authService.readRole;

  public searchString: string;
  paginationPages: number[][] = [];
  pageTotalCount: number;

  path: string[] = ['approvalEntries'];
  order = -1; // 1 asc, -1 desc

  constructor(private dataStorageService: DataStorageService,
              private approvalsService: ApprovalsService,
              private authService: AuthenticationService) { }

  ngOnInit() {
    this.setPage(0);

    this.paginationSubscription = this.approvalsService.pageEntriesChanged
    .subscribe(
      (pageEntries: Pagination) => {
        this.paginationPages = [];
        for (let i = 0; i < pageEntries.totalPages; i++) {
          this.paginationPages[i] = [i];
        }
        this.pageTotalCount = pageEntries.totalCount;
      }
    );

    this.subscription = this.approvalsService.entriesChanged
      .subscribe(
        (entries: Approval[]) => {
          this.approvalEntries = entries;
        }
      );
  }

  search(value) {
    this.setPage(0);
  }

  ngOnDestroy() {
    this.paginationSubscription.unsubscribe();
    this.subscription.unsubscribe();
  }

  setPage(page: number) {
    if (!this.searchString) {
      this.approvalsService.getEntries(page, 30);
    } else {
      this.approvalsService.getEntries(page, 30, this.searchString);
    }
    scrollTo(0, 0);
  }

}
