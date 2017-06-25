import { autoinject } from 'aurelia-framework';
import { AccountQueryService } from '../../services/query/account-query-service';

@autoinject
export class LoginPage {
    private accountQueryService: AccountQueryService;
    public providers = [];

    title = 'Login';
    message = 'Select a login provider to authenticate with.';
    returnUrl = '/';

    constructor(accountQueryService: AccountQueryService) {
        this.accountQueryService = accountQueryService;
    }

    activate() {
        this.accountQueryService.getExternalLoginProviders().then(providers => {
            this.providers = providers;
        });
    }
}