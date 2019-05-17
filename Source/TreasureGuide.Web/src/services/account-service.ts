import { autoinject, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { IMyProfileModel, IProfileDetailModel, RoleConstants } from '../models/imported';
import { DialogService } from 'aurelia-dialog';
import { AlertDialog, AlertDialogViewModel } from '../custom-elements/dialogs/alert-dialog';

@autoinject
export class AccountService {
    private router: Router;
    private dialog: DialogService;
    
    public userProfile: IMyProfileModel;

    constructor(router: Router, dialog: DialogService) {
        this.router = router;
        this.dialog = dialog;

        this.loadProfile();
    }

    private loadProfile() {
        var info = localStorage['user_profile'];
        if (info) {
            var deserialized = JSON.parse(info);
            this.userProfile = deserialized;
        } else if (localStorage['access_token']) {
            this.logout(true);
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

    public login() {
        var loc = '/Account/Login';
        var instruction = window.location.hash;
        if (instruction) {
            loc += ('?returnUrl=' + encodeURIComponent('/' + instruction));
        }
        window.location.href = loc;
    }

    public logout(force?: boolean) {
        this.dialog.open({
            viewModel: AlertDialog,
            model: <AlertDialogViewModel>{
                message: force ? 'You have been logged out.' : 'Are you sure you want to log out?',
                title: 'Logout',
                cancelable: !force,
                okayMessage: force ? 'Okay' : 'Yes',
                cancelMessage: 'No'
            },
            lock: true
        }).whenClosed(result => {
            if (!result.wasCancelled) {
                localStorage.clear();
                window.location.href = '/Account/Logout';
            }
        });
    }
    
    public static allRoles: string[] = [
        RoleConstants.Administrator,
        RoleConstants.Moderator,
        RoleConstants.BetaTester,
        RoleConstants.Contributor,
        RoleConstants.BoxUser,
        RoleConstants.Donor,
        RoleConstants.GCRAdmin,
        RoleConstants.GCRViewer
    ];
}