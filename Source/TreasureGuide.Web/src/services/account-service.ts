import { autoinject } from 'aurelia-dependency-injection';
import { HttpEngine } from '../tools/http-engine';

@autoinject
export class AccountService {
    private http: HttpEngine;

    public userInfo: UserInfoModel;

    constructor(http: HttpEngine) {
        this.http = http;
    }

    get isLoggedIn() {
        return false;
    }
}

export class UserInfoModel {
    public name: string;
}