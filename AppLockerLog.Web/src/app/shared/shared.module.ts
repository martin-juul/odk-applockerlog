import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FilterPipe } from './filter.pipe';
import { SortPipe } from '../log-entry/sort.pipe';

@NgModule({
    declarations: [
        FilterPipe,
        SortPipe
    ],
    exports: [
        CommonModule,
        FilterPipe,
        SortPipe
    ]
})
export class SharedModule {}
