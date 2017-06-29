import { autoinject } from 'aurelia-framework';
import { AccountQueryService } from '../../services/query/account-query-service';
import { AccountService } from '../../services/account-service';
import { Router } from 'aurelia-router';

@autoinject
export class LoginPage {
    private accountQueryService: AccountQueryService;
    private accountService: AccountService;
    private router: Router;
    public providers = [];

    title = 'Login';
    message = 'Select a login provider to authenticate with.';
    returnUrl = '/';

    constructor(accountQueryService: AccountQueryService, accountService: AccountService, router: Router) {
        this.accountQueryService = accountQueryService;
        this.accountService = accountService;
        this.router = router;
    }

    activate(params) {
        if (params.token) {
            this.accountService.setToken(params.token);
            if (params.returnUrl) {
                this.router.navigate(this.returnUrl);
            }
        }
        this.accountQueryService.getExternalLoginProviders().then(results => {
            this.providers = results;
        });
    }
}