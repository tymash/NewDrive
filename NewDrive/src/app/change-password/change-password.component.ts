import { Component, OnInit } from '@angular/core';
import { UserChangePasswordModel } from '../models/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  userId!: string;
  userChangePasswordModel: UserChangePasswordModel = {
    id: this.userId,
    password: ''
  };
  isPasswordError: boolean = false;
  isPasswordValidError: boolean = false;
  isPasswordSuccess: boolean = false;
  repeatPassword: string = '';

  constructor(private userService: UserService) { }

  ngOnInit(): void {
  }

  changePassword(validator: boolean | null) {
    this.isPasswordValidError = false;
    this.isPasswordError = false;
    this.isPasswordSuccess = false;

    if (!validator) {
      this.isPasswordValidError = true;
      return
    }

    if (this.userChangePasswordModel.password != this.repeatPassword) {
      this.isPasswordError = true;
      this.isPasswordSuccess = false;
      return
    }

    this.userService.changePassword(this.userChangePasswordModel)
      .subscribe({
        error: e => {
          this.isPasswordValidError = true;
          this.isPasswordSuccess = false;
        },
        complete: () => {
          this.isPasswordError = false;
          this.isPasswordValidError = false;
          this.isPasswordSuccess = true;
        }
      });
  }


}
