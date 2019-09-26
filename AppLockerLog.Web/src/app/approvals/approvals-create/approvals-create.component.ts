import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DataStorageService } from '../../shared/data-storage.service';
import { ApprovalsService } from '../approvals.service';
import { Router } from '@angular/router';
import { ApprovalsGroups } from './../approvals.groups';
import { AuthenticationService } from '../../auth/authentication.service';

@Component({
  selector: 'app-approvals-create',
  templateUrl: './approvals-create.component.html',
  styleUrls: ['./approvals-create.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ApprovalsCreateComponent implements OnInit {
  @ViewChild('f') approvalForm: NgForm;
  approval = {
    username: '',
    computerName: '',
    assignedUserGroups: <any>[],
    reasoning: ''
  };
  submitted = false;

  writeRole = this.authService.writeRole;

  constructor(
            private dataStorageService: DataStorageService,
            private approvalsService: ApprovalsService,
            private router: Router,
            private approvalsGroups: ApprovalsGroups,
            private authService: AuthenticationService) { }

  groups = this.approvalsGroups.getGroups();

  onSubmit() {
    const groups = this.approvalForm.value.groups;
    Object.keys(groups).forEach(
      k => (!groups[k] && groups[k] !== undefined) && delete groups[k]
    );
    const keys = Object.keys(groups);
    const groupArr = [];

    keys.forEach(element => {
      groupArr.push({'group': element });
    });

    this.submitted = true;
    this.approval.username = this.approvalForm.value.username;
    this.approval.assignedUserGroups = groupArr;
    this.approval.computerName = this.approvalForm.value.computername;
    this.approval.reasoning = this.approvalForm.value.reasoning;

    this.saveData();
  }

  saveData() {
    this.dataStorageService.post('approval', this.approval);
    setTimeout(() => {
      this.router.navigateByUrl('/approvals');
    }, 500);
  }

  ngOnInit() {
  }

}
