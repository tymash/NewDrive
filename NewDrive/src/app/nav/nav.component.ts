import { Component, Directive, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

export class NavComponent implements OnInit {
  isLoginWindow: boolean = false;
  isAuthenticated: boolean = false;
  userRole: string = '';
  name: string = '';
  surname: string = '';
  userId: string = '';

  constructor(private router: Router, private userService: UserService, private authService: AuthService) { }

  ngOnInit(): void {
    this.isAuthenticated = this.userService.isUserAuthenticated();
    this.userRole = this.userService.getAuthenticatedUserRole();
    this.userId = this.userService.getAuthenticatedUserId();
    if (this.userId) this.userService.getCurrentUser()
    .subscribe((userModel) => {
      this.name = userModel.name;
      this.surname = userModel.surname;
    })
  }

  public switchLogin() {
    this.isLoginWindow = !this.isLoginWindow;
  }

  public openProfile() {
    this.router.navigate(['my-profile']);
  }

  logout() {
    this.userService.logout();
    this.router.navigate(['']).then(() => window.location.reload());
  }

  login() {
    this.router.navigate(['/login']);
  }

}
