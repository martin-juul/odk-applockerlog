import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ApprovalsComponent } from './approvals.component';
import { ApprovalsCreateComponent } from './approvals-create/approvals-create.component';

const approvalsRoutes: Routes = [
    { path: '', component: ApprovalsComponent },
    { path: 'create', component: ApprovalsCreateComponent }
];

@NgModule({
    imports: [
        RouterModule.forChild(approvalsRoutes)
    ],
    exports: [RouterModule]
})
export class ApprovalsRoutingModule { }
