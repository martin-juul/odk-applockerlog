import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LogEntryComponent } from './log-entry.component';

const logEntryRoutes: Routes = [
    { path: '', component: LogEntryComponent }
];

@NgModule({
    imports: [
        RouterModule.forChild(logEntryRoutes)
    ],
    exports: [RouterModule]
})
export class LogEntryRoutingModule { }
