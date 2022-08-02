import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {UserChangePasswordModel, UserEditModel} from '../models/user.model';
import {UserService} from '../services/user.service';

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
        id: this.currentUserId,
        password: ''
    };
    isError: boolean = false;
    isPasswordError: boolean = false;
    isPasswordSuccess: boolean = false;
    isSuccess: boolean = false;
    isRepeatError: boolean = false;

    userRole: string = '';
    repeatPassword: string = '';

    constructor(private router: Router, private userService: UserService, private route: ActivatedRoute) {
        this.userRole = this.userService.getAuthenticatedUserRole();
        this.route.params.subscribe(params => {
                if (params['id'] && this.userRole == "Administrator") this.userId = params['id'];
            }
        );
    }

    public ngOnInit(): void {
        this.currentUserId = this.userService.getAuthenticatedUserId();
        if (this.userId == undefined) this.userId = this.currentUserId;

        if (this.userId != this.currentUserId) {
            this.userService.getById(this.userId)
                .subscribe((userModel) => {
                    this.userEditModel.name = userModel.name;
                    this.userEditModel.surname = userModel.surname;
                    this.userEditModel.email = userModel.email;
                });
        } else {
            this.userService.getCurrentUser()
                .subscribe((userModel) => {
                    this.userEditModel.name = userModel.name;
                    this.userEditModel.surname = userModel.surname;
                    this.userEditModel.email = userModel.email;
                });
        }

        this.userChangePasswordModel.id = this.currentUserId;

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
        this.isPasswordError = false;
        this.isRepeatError = false;
        this.isPasswordSuccess = false;

        if (!validator) {
            this.isPasswordError = true;
            return
        }

        if (this.userChangePasswordModel.password != this.repeatPassword) {
            this.isRepeatError = true;
            this.isPasswordSuccess = false;
            return
        }

        this.userService.changePassword(this.userChangePasswordModel)
            .subscribe({
                error: e => {
                    console.log(e);
                    this.isPasswordSuccess = false;
                    this.isPasswordError = true;
                },
                complete: () => {
                    this.isPasswordError = false;
                    this.isRepeatError = false;
                    this.isPasswordSuccess = true;
                }
            });
    }

}
