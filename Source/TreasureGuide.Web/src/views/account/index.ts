import { autoinject, bindable, computedFrom } from 'aurelia-framework';
import { AccountService } from '../../services/account-service';

@autoinject
export class AccountIndexPage {
    private accountService: AccountService;
    title = 'Account Profile';

    constructor(accountService: AccountService) {
        this.accountService = accountService;
    }
}