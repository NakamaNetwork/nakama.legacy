import { autoinject, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';

@autoinject
export class AccountService {
    private router: Router;
    public userProfile: any;

    constructor(router: Router) {
        this.router = router;
        this.loadProfile();
    }

    private loadProfile() {
        var info = sessionStorage['user_profile'];
        if (info) {
            var deserialized = JSON.parse(info);
            this.userProfile = deserialized;
        }
    }

    isInRole(role: string): boolean {
        return this.isInRoles([role]);
    }

    isInRoles(roles: string[]): boolean {
        if (roles.length === 0) {
            return this.isLoggedIn;
        } else {
            for (var i = 0; i < roles.length; i++) {
                var userRoles = this.userProfile.roles.map(r => r.toLowerCase());
                if (userRoles.indexOf(roles[i]) === -1) {
                    return false;
                }
            }
        }
        return true;
    }

    @computedFrom('userInfo')
    get isLoggedIn(): boolean {
        if (this.userProfile) {
            return true;
        }
        return false;
    }

    public logout() {
        sessionStorage.clear();
        window.location.href = '/Account/Logout';
    }
}