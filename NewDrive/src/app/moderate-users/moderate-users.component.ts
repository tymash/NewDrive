import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { UserViewModel } from '../models/user.model';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-moderate-users',
  templateUrl: './moderate-users.component.html',
  styleUrls: ['./moderate-users.component.css']
})
export class ModerateUsersComponent implements OnInit {
  users?: UserViewModel[];
  bsModalRef?: BsModalRef;

  constructor(private userService: UserService,
    private modalService: BsModalService, private router: Router) { }

  ngOnInit() {
    if (this.userService.getAuthenticatedUserRole() != "Administrator") this.router.navigate(['']);
    this.getUsers();
  }

  getUsers() {
    this.userService.getAll()
      .subscribe(users => this.users = users);
  }


}
