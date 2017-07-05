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
        if (authorizations.length > 0) {
            var roles = new Array<string>();
            authorizations.forEach(a => {
                if (Array.isArray(a)) {
                    a.forEach(r => roles.push(r));
                } else if (typeof (a) === 'string') {
                    roles.push(a);
                }
            });
            if (this.accountService.isInRoles(roles)) {
                return next();
            }
        } else {
            return next();
        }
        return next.cancel(new Redirect(routingContext.router.generate('unauthorized')));
    }
}