import { Component, OnInit, ViewEncapsulation, Input, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ApprovalsGroups } from './../approvals.groups';
import { ApprovalsService } from './../approvals.service';
import { Approval } from './../../shared/approval.model';

import { AuthenticationService } from '../../auth/authentication.service';
import { DataStorageService } from './../../shared/data-storage.service';

@Component({
  selector: '[app-approvals-item]',
  templateUrl: './approvals-item.component.html',
  styleUrls: ['./approvals-item.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ApprovalsItemComponent implements OnInit {
  @Input() entries: Approval;
  @Input() index: number;
  @ViewChild('f') groupForm: NgForm;
  writeRole = this.authService.writeRole;
  endpoint = 'approval';
  submitted = false;

  constructor(
    private approvalsService: ApprovalsService,
    private authService: AuthenticationService,
    private dataStorageService: DataStorageService,
    private approvalsGroups: ApprovalsGroups) { }
    groups = [];

  ngOnInit() {
    this.groups = this.approvalsGroups.getGroups();
  }

  deleteEntry(id: number): void {
    if (confirm('Are you sure you want to delete the request with id: ' + id + '?')) {
        this.approvalsService.delete(id);
    }
  }

  addGroup(id: number) {
    this.submitted = true;
    const group = this.groupForm.value.group;
    this.approvalsService.addGroup(id, group);
  }

  removeGroupFromUser(groupId: number, entryId: number, group: string, userName: string) {
    if (confirm('Are you sure you want to remove the group ' + group + ' from ' + userName + '?')) {
      this.approvalsService.deleteGroup(groupId, entryId);
    }
  }

  captureChange(id: number, value: string): void {
    setTimeout(() => {
        this.approvalsService.updateNote(id, value);
    }, 500);
  }

  captureComputerNameChange(id: number, value: string) {
    setTimeout(() => {
      this.approvalsService.updateComputerName(id, value);
  }, 500);
  }

}
