import { autoinject } from 'aurelia-framework';
import { AccountService } from '../services/account-service';
import { NavigationInstruction, Redirect } from 'aurelia-router';

@autoinject
export class AuthorizeStep {
    private accountService: AccountService;

    constructor(accountService: AccountService) {
        this.accountService = accountService;
    }

    run(routingContext: NavigationInstruction, next) {
        var authorizations = routingContext.getAllInstructions().map(i => i.config['auth']).filter(i => i);
        if (authorizations.every(a => this.accountService.isInRoles(a))) {
            return next();
        }
        return next.cancel(new Redirect(routingContext.router.generate('unauthorized')));
    }
}