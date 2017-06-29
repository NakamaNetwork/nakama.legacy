import { autoinject, computedFrom } from 'aurelia-framework';
import { AccountQueryService } from './query/account-query-service';

@autoinject
export class AccountService {
    private accountQueryService: AccountQueryService;

    public userInfo: UserInfoModel;

    constructor(accountQueryService: AccountQueryService) {
        this.accountQueryService = accountQueryService;
        this.setToken(localStorage['access_token']);
    }

    setToken(newValue) {
        localStorage['access_token'] = newValue;
        if (newValue) {
            this.accountQueryService.getUserInfo().then(result => {
                this.userInfo = result;
            });
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