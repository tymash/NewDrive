import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { UserLoginModel } from '../models/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @Output() switchEvent = new EventEmitter();
  isError: boolean = false;
  userLoginModel: UserLoginModel = {
    email: '',
    password: ''
  }

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit(): void {
  }

  callParentSwitch() {
    this.switchEvent.emit();
  }

  loginUser(validator: boolean | null) {
    if (!validator) {
      this.isError = true;
      return;
    }

    this.userService.login(this.userLoginModel)
      .subscribe({
        next: response => {
          const token = response.token;
          sessionStorage.setItem('token', token);
          this.isError = false;
          this.router.navigate([''])
            .then(() => window.location.reload());
        },
        error: e => {
          this.isError = true;
        }
      });
  }

}
