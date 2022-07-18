import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpParams, HttpRequest } from '@angular/common/http';
import { saveAs } from 'file-saver';
import { JwtHelperService } from '@auth0/angular-jwt';
import { FileCreateModel, FileViewModel } from '../models/file.model';
import { Observable } from 'rxjs';
import { FilterModel } from '../models/filter.model';
import { UserChangePasswordModel, UserEditModel, UserLoginModel, UserRegisterModel, UserViewModel } from '../models/user.model';

@Injectable()
export class UserService {
  private url = "https://localhost:7083/api/users";

  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) { }

  register(userRegisterModel: UserRegisterModel) {
    return this.http.post<UserViewModel>(this.url + '/register', userRegisterModel);
  }

  login(userLoginModel: UserLoginModel) {
    return this.http.post<UserViewModel>(this.url + '/login', userLoginModel);
  }

  logout() {
    sessionStorage.removeItem('token');
    return this.http.post(this.url + '/logout', null);
  }

  update(userId: string, userModel: UserEditModel) {
    return this.http.put(this.url + '/edit/' + userId, userModel);
  }

  changePassword(userModel: UserChangePasswordModel) {
    return this.http.put(this.url + '/change-password', userModel);
  }

  getById(userId: string) {
    return this.http.get<UserViewModel>(this.url + '/' + userId);
  }

  getAll() {
    return this.http.get<UserViewModel[]>(this.url);
  }

  getCurrentUser() {
    return this.http.get<UserViewModel>(this.url + '/current');
  }

  isUserAuthenticated() {
    const token = sessionStorage.getItem('token');

    if (token && !this.jwtHelper.isTokenExpired(token))
      return true;

    return false;
  }

  getAuthenticatedUserRole() {
    const token = sessionStorage.getItem('token');
    const roleClaims: string = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

    if (token && this.isUserAuthenticated()) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return decodedToken[roleClaims];
    }
    else
      return '';
  }

  getAuthenticatedUserId() {
    const token = sessionStorage.getItem('token');
    const identifierClaims: string = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';

    if (token && this.isUserAuthenticated()) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return decodedToken[identifierClaims];
    }
    else
      return '';
  }

}
