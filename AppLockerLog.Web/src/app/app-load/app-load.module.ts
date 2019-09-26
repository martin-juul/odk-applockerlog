import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppLoadService } from './app-load.service';

export function getUser(appLoadService: AppLoadService) {
    return () => appLoadService.getUser();
}

@NgModule({
    imports: [HttpClientModule],
    providers: [
        AppLoadService,
        { provide: APP_INITIALIZER, useFactory: getUser, deps: [AppLoadService], multi: true },
    ]
})
export class AppLoadModule { }
