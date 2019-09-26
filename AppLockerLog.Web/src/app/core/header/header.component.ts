import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DataStorageService } from '../../shared/data-storage.service';
import { AuthenticationService } from './../../auth/authentication.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class HeaderComponent implements OnInit {
    userName: string;
    consumerRole = this.authService.consumerRole;
    readRole = this.authService.readRole;

    constructor(private dataStorageService: DataStorageService,
                private authService: AuthenticationService) { }

    ngOnInit() {
        this.userName = this.dataStorageService.getUserName();
    }
}
