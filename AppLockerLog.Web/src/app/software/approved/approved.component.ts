import { ApprovalsService } from './../../approvals/approvals.service';
import {
  Component,
  OnInit,
  OnDestroy,
  ViewEncapsulation
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { AuthenticationService } from './../../auth/authentication.service';
import { DataStorageService } from './../../shared/data-storage.service';

import { Software } from '../software-model';
import { SoftwareService } from '../software.service';
import { Pagination } from '../../shared/pagination.model';

@Component({
  selector: 'app-approved',
  templateUrl: './approved.component.html',
  styleUrls: ['./approved.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ApprovedComponent implements OnInit, OnDestroy {
  approvedSoftware: Software[];
  softwareSubscription: Subscription;
  paginationSubscription: Subscription;

  path: string[] = ['approvedSoftware'];
  order = -1; // 1 asc, -1 desc
  paginationPages: number[][] = [];
  pageTotalCount: number;

  writeRole = this.authService.writeRole;
  readRole = this.authService.readRole;

  public searchString: string;


  constructor(private softwareService: SoftwareService,
              private route: ActivatedRoute,
              private router: Router,
              private authService: AuthenticationService) { }

  ngOnInit() {
    this.setPage(0);

    this.paginationSubscription = this.softwareService.pageEntriesChanged
      .subscribe(
        (pageEntries: Pagination) => {
          this.paginationPages = [];
          for (let i = 0; i < pageEntries.totalPages; i++) {
            this.paginationPages[i] = [i];
          }
          this.pageTotalCount = pageEntries.totalCount;
        }
      );

    this.softwareSubscription = this.softwareService.entriesChanged
      .subscribe(
        (entries: Software[]) => {
          this.approvedSoftware = entries;
        }
      );
  }

  search(value) {
    this.setPage(0);
  }

  sortTable(prop: string) {
    this.path = prop.split('.');
    this.order = this.order * (-1);
  }

  ngOnDestroy() {
    this.paginationSubscription.unsubscribe();
    this.softwareSubscription.unsubscribe();
  }

  setPage(page: number) {
    const state = 'approved';

    if (!this.searchString) {
      this.softwareService.getEntries(page, 30, state);
    } else {
      this.softwareService.getEntries(page, 30, state, this.searchString);
    }
    scrollTo(0, 0);
  }

}
