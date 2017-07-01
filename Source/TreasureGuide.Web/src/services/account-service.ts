import { autoinject, computedFrom } from 'aurelia-framework';

@autoinject
export class AccountService {
    public userInfo: UserInfoModel;

    constructor() {
        this.loadProfile();
    }

    private loadProfile() {
        var info = sessionStorage['user_info'];
        if (info) {
            var deserialized = JSON.parse(info) as UserInfoModel;
            this.userInfo = deserialized;
        }
    }

    @computedFrom('userInfo')
    get isLoggedIn(): boolean {
        if (this.userInfo) {
            return true;
        }
        return false;
    }
}

export class UserInfoModel {
    public name: string;
}