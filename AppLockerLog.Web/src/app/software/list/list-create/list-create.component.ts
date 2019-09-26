import {
    Component,
    OnInit,
    ViewEncapsulation,
    ViewChild
} from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { SoftwareService } from '../../software.service';
import { DataStorageService } from '../../../shared/data-storage.service';
import { AuthenticationService } from './../../../auth/authentication.service';

@Component({
    selector: 'app-list-create',
    templateUrl: './list-create.component.html',
    styleUrls: ['./list-create.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class ListCreateComponent implements OnInit {
    @ViewChild('f') itemForm: NgForm;
    submitted = false;
    listItem = {
        name: '',
        vendor: '',
        reasoning: '',
        state: ''
    };

    writeRole = this.authService.writeRole;

    constructor(private softwareService: SoftwareService,
                private router: Router,
                private dataStorageService: DataStorageService,
                private authService: AuthenticationService) { }

    ngOnInit() {

    }

    onSubmit() {
        this.submitted = true;

        this.listItem.name = this.itemForm.value.name;
        this.listItem.vendor = this.itemForm.value.vendor;
        this.listItem.reasoning = this.itemForm.value.reasoning;
        this.listItem.state = this.itemForm.value.state;

        this.saveData();
    }

    saveData() {
        this.dataStorageService.post('software', this.listItem);

        setTimeout(() => {
            this.router.navigateByUrl('/software/approved');
        }, 500);
    }
}
