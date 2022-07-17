import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FilesComponent } from './files/files.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RecycleBinComponent } from './recycle-bin/recycle-bin.component';
import {RegistrationComponent} from "./registration/registration.component";
import { AuthService } from './services/auth.service';
import {NonAuthService} from "./services/non-auth.service";
import { SessionErrorComponent } from './session-error/session-error.component';
import { StorageComponent } from './storage/storage.component';

const routes: Routes = [
  { path: '', component: HomeComponent},
  // {
  //   path: '',
  //   runGuardsAndResolvers: 'always',
  //   canActivate: [AuthGuard],
  //   children: [
  //     { path: 'storage-items', component: StorageTabsComponent },
  //     { path: 'shared-items', component: SharedItemsComponent },
  //     { path: 'recycle-bin', component: RecycleBinComponent },
  //     { path: 'admin', component: AdminPanelComponent, data: { roles: ['Admin'] } },
  //     { path: 'moderate', component: ModerateItemsComponent, data: { roles: ['Moderator'] } }
  //   ]
  // },
  { path: 'files', component: StorageComponent},
  { path: 'session-error', component: SessionErrorComponent},
  { path: 'recycle-bin', component: RecycleBinComponent },
  { path: 'register', component: RegistrationComponent, canActivate: [NonAuthService]},
  { path: 'login', component: LoginComponent, canActivate: [NonAuthService] },
  { path: '**', component: HomeComponent }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
