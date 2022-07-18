import {NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';
import {BrowserModule} from '@angular/platform-browser';
import { JwtModule } from "@auth0/angular-jwt";
import { RegistrationComponent } from './registration/registration.component';
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FileUploadModule } from 'ng2-file-upload';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsModalRef, ModalModule } from 'ngx-bootstrap/modal';
import { CollapseModule } from 'ngx-bootstrap/collapse';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {HomeComponent} from './home/home.component';
import {UserService} from "./services/user.service";
import {AuthService} from "./services/auth.service";
import {NonAuthService} from "./services/non-auth.service";
import { NavComponent } from './nav/nav.component';
import { LoginComponent } from './login/login.component';
import { FilesComponent } from './files/files.component';
import { FileService } from './services/file.service';
import { UploadComponent } from './upload/upload.component';
import { StorageComponent } from './storage/storage.component';
import { RecycleBinComponent } from './recycle-bin/recycle-bin.component';
import { SessionErrorComponent } from './session-error/session-error.component';
import { PublicComponent } from './public/public.component';
import { SharedDownloadComponent } from './shared-download/shared-download.component';
import { ProfileComponent } from './profile/profile.component';
import { ModerateFilesComponent } from './moderate-files/moderate-files.component';
import { ModerateUsersComponent } from './moderate-users/moderate-users.component';
import { ChangePasswordComponent } from './change-password/change-password.component';

export function getToken() {
  return sessionStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    RegistrationComponent,
    NavComponent,
    LoginComponent,
    FilesComponent,
    UploadComponent,
    StorageComponent,
    RecycleBinComponent,
    SessionErrorComponent,
    PublicComponent,
    SharedDownloadComponent,
    ProfileComponent,
    ModerateFilesComponent,
    ModerateUsersComponent,
    ChangePasswordComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FileUploadModule,
    TabsModule,
    ModalModule.forRoot(),
    CollapseModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: getToken,
        allowedDomains: ["localhost:7083"],
        disallowedRoutes: []
      }
    }),
    FormsModule,
    BrowserAnimationsModule,
    FontAwesomeModule,
    CommonModule
  ],
  providers: [AuthService, NonAuthService, UserService, FileService, BsModalRef],
  bootstrap: [AppComponent]
})
export class AppModule { }
