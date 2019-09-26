import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { FormsModule } from '@angular/forms';
import { AppLoadModule } from './app-load/app-load.module';

import { ApprovalsService } from './approvals/approvals.service';
import { AuthenticationService } from './auth/authentication.service';
import { AuthGuard } from './auth/auth-guard.service';
import { NtlmInterceptor } from './auth/ntlm-interceptor';
import { PaginatorService } from './shared/paginator.service';

import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    CoreModule,
    FormsModule,
    HttpClientModule,
    HttpModule,
    SharedModule,
    AppLoadModule
  ],
  providers: [
      {
          provide: HTTP_INTERCEPTORS,
          useClass: NtlmInterceptor,
          multi: true
      },
      AuthenticationService,
      AuthGuard,
      PaginatorService,
      ApprovalsService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
