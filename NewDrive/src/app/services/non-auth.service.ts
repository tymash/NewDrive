import { Injectable } from "@angular/core";
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";

@Injectable()
export class NonAuthService implements CanActivate {

  constructor(private router: Router, private jwtHelper: JwtHelperService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const token = sessionStorage.getItem("token");

    if (token == null) {
      return true;
    }

    this.router.navigateByUrl('/');
    return false;
  }
}
