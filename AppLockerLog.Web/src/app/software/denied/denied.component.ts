import {
  Component,
  OnInit,
  OnDestroy,
  ViewEncapsulation
} from '@angular/core';

import { Software } from '../software-model';
import { Pagination } from './../../shared/pagination.model';

import { SoftwareService } from '../software.service';
import { AuthenticationService } from '../../auth/authentication.service';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-denied',
  templateUrl: './denied.component.html',
  styleUrls: ['./denied.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeniedComponent implements OnInit, OnDestroy {
  deniedSoftware: Software[];
  softwareSubscription: Subscription;
  paginationSubscription: Subscription;

  writeRole = this.authService.writeRole;
  readRole = this.authService.readRole;

  public searchString: string;
  paginationPages: number[][] = [];
  pageTotalCount: number;

  path: string[] = ['deniedSoftware'];
  order = -1; // 1 asc, -1 desc

  constructor(private softwareService: SoftwareService,
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
          this.deniedSoftware = entries;
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
    const state = 'denied';

    if (!this.searchString) {
      this.softwareService.getEntries(page, 30, state);
    } else {
      this.softwareService.getEntries(page, 30, state, this.searchString);
    }
    scrollTo(0, 0);
  }
}
