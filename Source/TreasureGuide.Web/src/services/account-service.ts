import { autoinject, computedFrom } from 'aurelia-framework';
import { ProfileQueryService } from './query/profile-query-service';

@autoinject
export class AccountService {
    private profileQueryService: ProfileQueryService;

    public userInfo: UserInfoModel;

    constructor(profileQueryService: ProfileQueryService) {
        this.profileQueryService = profileQueryService;
        this.profileQueryService.getProfile().then(result => {
            this.userInfo = result;
        });
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