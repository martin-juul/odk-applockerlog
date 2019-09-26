import { Component, OnInit, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { Software } from './../../software-model';

import { SoftwareService } from '../../software.service';
import { DataStorageService } from '../../../shared/data-storage.service';
import { AuthenticationService } from './../../../auth/authentication.service';

@Component({
    selector: 'app-list-edit',
    templateUrl: './list-edit.component.html',
    styleUrls: ['./list-edit.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class ListEditComponent implements OnInit {
    @Input() entry: Software;
    writeRole = this.authService.writeRole;

    constructor(private authService: AuthenticationService) { }

    ngOnInit() {
    }

    onSubmit() {

    }
}
