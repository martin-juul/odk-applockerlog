import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { SoftwareRoutingModule } from './software-routing.module';
import { SharedModule } from '../shared/shared.module';

import { SoftwareService } from './software.service';
import { AuthenticationService } from '../auth/authentication.service';

import { ApprovedComponent } from './approved/approved.component';
import { DeniedComponent } from './denied/denied.component';
import { ListItemComponent } from './list/list-item.component';
import { ListCreateComponent } from './list/list-create/list-create.component';
import { ListEditComponent } from './list/list-edit/list-edit.component';

@NgModule({
    declarations: [
        ApprovedComponent,
        DeniedComponent,
        ListItemComponent,
        ListCreateComponent,
        ListEditComponent
    ],
    imports: [
        FormsModule,
        RouterModule,
        SoftwareRoutingModule,
        SharedModule
    ],
    providers: [
        SoftwareService
    ]
})
export class SoftwareModule { }
