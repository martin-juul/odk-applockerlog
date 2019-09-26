import {
    Component,
    OnInit,
    ViewEncapsulation,
    Input,
    Output,
    EventEmitter
} from '@angular/core';

import { AuthenticationService } from '../../auth/authentication.service';

import { Software } from '../software-model';
import { SoftwareService } from '../software.service';

@Component({
    selector: '[list-item]',
    templateUrl: './list-item.component.html',
    styleUrls: ['./list-item.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class ListItemComponent implements OnInit {
    @Input() entry: Software;
    @Input() index: number;
    writeRole = this.authService.writeRole;
    readRole = this.authService.readRole;
    consumerRole = this.authService.consumerRole;

    constructor(private authService: AuthenticationService,
                private softwareService: SoftwareService) { }

    ngOnInit() {
    }

    deleteEntry(id: number) {
        if (confirm('Are you sure you want to delete the item with id: ' + id)) {
            this.softwareService.deleteEntry(id);
        }
    }

    captureChange(id: number, value: string) {
        setTimeout(() => {
            this.softwareService.updateReasoning(id, value);
        }, 500);
    }

}
