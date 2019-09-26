import { Injectable } from '@angular/core';

@Injectable()
export class ApprovalsGroups {
    groups = [
        'Deny Applocker',
        'Local Administrator'
      ];

    getGroups() {
        return this.groups;
    }

}
