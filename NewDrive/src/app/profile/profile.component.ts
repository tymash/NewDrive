import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserChangePasswordModel, UserEditModel } from '../models/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  
  currentUserId!: string;
  userId!: string;
  userEditModel: UserEditModel = {};
  userChangePasswordModel: UserChangePasswordModel = {
    id: this.userId,
    password: ''
  };
  isError: boolean = false;
  isSuccess: boolean = false;
  isPasswordError: boolean = false;

  userRole: string = '';
  repeatPassword: string = '';

  constructor(private router: Router, private userService: UserService, private route: ActivatedRoute) { 
    this.route.params.subscribe(params => {
      if (params['id']) this.userId = params['id'];
    }
    );
  }

  public ngOnInit(): void {
    if (this.userId == undefined) this.userId = this.userService.getAuthenticatedUserId();
    this.currentUserId = this.userService.getAuthenticatedUserId();
    this.userRole = this.userService.getAuthenticatedUserRole();

    this.userService.getById(this.userId)
      .subscribe((userModel) => {
        this.userEditModel.name = userModel.name;
        this.userEditModel.surname = userModel.surname;
        this.userEditModel.email = userModel.email;
      });

  }

  public logout(): void {
    this.userService.logout();
    this.router.navigate([''])
      .then(() => window.location.reload());
  }

  saveChanges(validator: boolean | null) {
    this.userEditModel.id = this.userId;
    if (this.userEditModel.email != undefined) {
      const isEmailValid = /^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$/g.test(this.userEditModel.email);
      validator = validator && (isEmailValid);
    }

    if (!validator) {
      this.isError = true;
      this.isSuccess = false;
      return
    }

    this.userService.update(this.userId, this.userEditModel)
      .subscribe({
        error: e => {
          console.log(e);
          this.isError = true;
          this.isSuccess = false;
        },
        complete: () => {
          this.isError = false;
          this.isSuccess = true;
          window.location.reload();
        }
      });
  }

  changePassword(validator: boolean | null) {
    this.isError = false;
    this.isPasswordError = false;
    this.isSuccess = false;

    if (!validator) {
      this.isError = true;
      return
    }

    if (this.userChangePasswordModel.password != this.repeatPassword) {
      this.isPasswordError = true;
      this.isSuccess = false;
      return
    }

    this.userService.changePassword(this.userChangePasswordModel)
      .subscribe({
        error: e => {
          this.isSuccess = true;
          this.isError = false;
        },
        complete: () => {
          this.isPasswordError = false;
          this.isError = false;
          this.isSuccess = true;
        }
      });
  }

}
