import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FilesComponent } from './files/files.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { ModerateFilesComponent } from './moderate-files/moderate-files.component';
import { ModerateUsersComponent } from './moderate-users/moderate-users.component';
import { ProfileComponent } from './profile/profile.component';
import { PublicComponent } from './public/public.component';
import { RecycleBinComponent } from './recycle-bin/recycle-bin.component';
import {RegistrationComponent} from "./registration/registration.component";
import { AuthService } from './services/auth.service';
import {NonAuthService} from "./services/non-auth.service";
import { SessionErrorComponent } from './session-error/session-error.component';
import { SharedDownloadComponent } from './shared-download/shared-download.component';
import { StorageComponent } from './storage/storage.component';

const routes: Routes = [
  { path: '', component: HomeComponent},
  { path: 'public', component: PublicComponent, canActivate: [AuthService]},
  { path: 'moderate-files', component: ModerateFilesComponent, canActivate: [AuthService] },
  { path: 'shared/:id/:name', component: SharedDownloadComponent},
  { path: 'files', component: StorageComponent, canActivate: [AuthService]},
  { path: 'session-error', component: SessionErrorComponent},
  { path: 'recycle-bin', component: RecycleBinComponent, canActivate: [AuthService]},
  { path: 'register', component: RegistrationComponent, canActivate: [NonAuthService]},
  { path: 'login', component: LoginComponent, canActivate: [NonAuthService]},
  { path: 'user-management', component: ModerateUsersComponent, canActivate: [AuthService]},
  { path: 'profile/:id', component: ProfileComponent, canActivate: [AuthService] },
  { path: 'my-profile', component: ProfileComponent, canActivate: [AuthService] },
  { path: '**', component: HomeComponent }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
