import { autoinject } from 'aurelia-framework';
import { AccountService } from '../../services/account-service';

@autoinject
export class IntroPage {
    private accountService: AccountService;

    constructor(accountService: AccountService) {
        this.accountService = accountService;
    }

    login() {
        this.accountService.login();
    }
}