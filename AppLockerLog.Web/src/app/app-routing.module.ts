import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';

import { AuthGuard } from './auth/auth-guard.service';

// , canActivate: [AuthGuard]

const appRoutes: Routes = [
    { path: '', loadChildren: './log-entry/log-entry.module#LogEntryModule', canActivate: [AuthGuard] },
    { path: 'approvals', loadChildren: './approvals/approvals.module#ApprovalsModule', canActivate: [AuthGuard] },
    { path: 'software', loadChildren: './software/software.module#SoftwareModule', canActivate: [AuthGuard] }
];

@NgModule({
    imports: [
        RouterModule.forRoot(appRoutes, {preloadingStrategy: PreloadAllModules})
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
