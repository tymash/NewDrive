import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {

  constructor(private router: Router,
    private authService: UserService) { }

  ngOnInit() {
  }

  moveToFiles() {
    this.router.navigate(['/files']);
  }

  loggedIn() {
    return this.authService.isUserAuthenticated();
  }

}