import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import {UserRegisterModel} from "../models/user.model";
import { faPenToSquare } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})

export class RegistrationComponent implements OnInit {

  userRegisterModel: UserRegisterModel = {
    name: "",
    surname: "",
    email: "",
    password: ""
  }
  repeatPassword: string = "";
  confirmation: boolean = false;
  isError: boolean = false;
  isPasswordError: boolean = false;
  isConfirmationError: boolean = false;
  isSuccess: boolean = false;

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
  }

  saveChanges(validator: boolean | null) {
    this.isError = false;
    this.isPasswordError = false;
    this.isConfirmationError = false;
    this.isSuccess = false;
    console.log('g')

    if (!validator) {
      console.log('c')
      this.isError = true;
      this.isSuccess = false;
      return
    }

    if (this.userRegisterModel.password != this.repeatPassword) {
      console.log('b')
      this.isPasswordError = true;
      return
    }

    this.userService.register(this.userRegisterModel)
      .subscribe({
        next: um => {
          sessionStorage.setItem('token', um.token);
        },
        error: e => {
          this.isError = true;
          this.isSuccess = false;
        },
        complete: () => {
          this.router.navigate([''])
            .then(() => window.location.reload());
        }
      });
  }


}
