﻿import { autoinject, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { IProfileAuthExportModel } from '../models/imported';
import { RoleConstants } from '../models/imported';

@autoinject
export class AccountService {
    private router: Router;
    public userProfile: IProfileAuthExportModel;

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

    isInRoles(authParams, all: boolean = false): boolean {
        if (!authParams || (Array.isArray(authParams) && authParams.length === 0)) {
            return true;
        }
        if (!this.isLoggedIn) {
            return false;
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

    @computedFrom('userInfo')
    get isLoggedIn(): boolean {
        if (this.userProfile) {
            return true;
        }
        return false;
    }

    @computedFrom('userInfo')
    private get lowerRoles() {
        return this.userProfile.roles.map(r => r.toLowerCase());
    }

    public logout() {
        sessionStorage.clear();
        window.location.href = '/Account/Logout';
    }

    static AllRoles: string[] = [
        RoleConstants.Administrator,
        RoleConstants.Moderator,
        RoleConstants.Contributor
    ];
}