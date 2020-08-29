import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { IdentityService } from '../../services/identity.service';

@Injectable({
    providedIn: 'root'
  })
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private service: IdentityService) {
    }

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): boolean {
        if (localStorage.getItem('token') != null) {
            const roles = next.data['permittedRoles'] as Array<string>;
            if (roles) {
                if (this.service.roleMatch(roles)) {
                    return true;
                } else {
                    this.router.navigate(['/403']);
                    return false;
                }
            }
            return true;
        } else {
            this.router.navigate(['/home']);
            return false;
        }

    }
}