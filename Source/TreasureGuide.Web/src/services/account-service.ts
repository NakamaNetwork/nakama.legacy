import { autoinject, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { IProfileDetailModel } from '../models/imported';
import { RoleConstants } from '../models/imported';

@autoinject
export class AccountService {
    private router: Router;
    public userProfile: IProfileDetailModel;

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

    isInRoles(authParams, all: boolean = false, req: boolean = false): boolean {
        if (!req && !authParams || (Array.isArray(authParams) && authParams.length === 0)) {
            return true;
        }
        if (!this.isLoggedIn) {
            return false;
        }
        if (!authParams || (Array.isArray(authParams) && authParams.length === 0)) {
            return true;
        }
        if (typeof (authParams) === 'string') {
            authParams = authParams.split(',');
        }
        if (Array.isArray(authParams)) {
            if (all) {
                return authParams.every(p => this.isInRole(p.toString()));
            } else {
                return authParams.some(p => this.isInRole(p.toString()));
            }
        }
        return true;
    }

    private isInRole(role: string): boolean {
        return this.lowerRoles.indexOf(role.toLowerCase()) > -1;
    }

    @computedFrom('userProfile')
    get isLoggedIn(): boolean {
        if (this.userProfile) {
            return true;
        }
        return false;
    }

    @computedFrom('userProfile')
    private get lowerRoles() {
        if (this.userProfile) {
            return this.userProfile.userRoles.map(r => r.toLowerCase());
        }
        return [];
    }

    public logout() {
        sessionStorage.clear();
        window.location.href = '/Account/Logout';
    }

    public static allRoles: string[] = [
        RoleConstants.Administrator,
        RoleConstants.Moderator,
        RoleConstants.Contributor
    ];
}