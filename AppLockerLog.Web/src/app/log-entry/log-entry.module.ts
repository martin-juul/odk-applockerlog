import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { LogEntryRoutingModule } from './log-entry-routing.module';
import { SharedModule } from '../shared/shared.module';

import { LogEntryComponent } from './log-entry.component';
import { LogEntryItemComponent } from './log-entry-item/log-entry-item.component';

@NgModule({
    declarations: [
        LogEntryComponent,
        LogEntryItemComponent
    ],
    imports: [
        FormsModule,
        LogEntryRoutingModule,
        RouterModule,
        SharedModule,
    ]
})
export class LogEntryModule { }
