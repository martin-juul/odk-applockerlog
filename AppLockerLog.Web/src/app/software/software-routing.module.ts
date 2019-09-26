import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ApprovedComponent } from './approved/approved.component';
import { DeniedComponent } from './denied/denied.component';
import { ListCreateComponent } from './list/list-create/list-create.component';
import { ListEditComponent } from './list/list-edit/list-edit.component';

const softwareRoutes: Routes = [
    { path: 'approved', component: ApprovedComponent },
    { path: 'denied', component: DeniedComponent },
    { path: 'create', component: ListCreateComponent },
    { path: 'edit', component: ListEditComponent }
];

@NgModule({
    imports: [
        RouterModule.forChild(softwareRoutes)
    ],
    exports: [RouterModule]
})
export class SoftwareRoutingModule { }
