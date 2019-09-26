import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { ApprovalsRoutingModule } from './approvals-routing.module';
import { SharedModule } from '../shared/shared.module';

import { ApprovalsComponent } from './approvals.component';
import { ApprovalsItemComponent } from './approvals-item/approvals-item.component';
import { ApprovalsCreateComponent } from './approvals-create/approvals-create.component';

import { ApprovalsGroups } from './approvals.groups';

@NgModule({
	// tslint:disable:indent
	declarations: [
		ApprovalsComponent,
		ApprovalsItemComponent,
		ApprovalsCreateComponent
	],
	imports: [
		FormsModule,
		RouterModule,
		SharedModule,
		ApprovalsRoutingModule
	],
	providers: [
		ApprovalsGroups
	]
})
export class ApprovalsModule { }
