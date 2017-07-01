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