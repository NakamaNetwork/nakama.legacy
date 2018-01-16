import { autoinject, computedFrom } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import { RouterConfiguration, Router } from 'aurelia-router';
import { AccountService } from './services/account-service';
import { AuthorizeStep } from './tools/authorize-step';

@autoinject
export class App {
    public router: Router;
    public message = 'Shukko da!';
    public accountService: AccountService;
    private ea: EventAggregator;

    constructor(accountService: AccountService, eventAggregator: EventAggregator) {
        this.accountService = accountService;
        this.ea = eventAggregator;
        this.ea.subscribe('router:navigation:complete', response => {
            this.navToggled = false;
        });
    }

    configureRouter(config: RouterConfiguration, router: Router): void {
        this.router = router;
        config.title = 'Nakama Network';
        config.addAuthorizeStep(AuthorizeStep);
        config.map([
            { route: ['', '/', '_=_', 'home'], name: 'home', title: 'Home', moduleId: 'views/index', nav: true },
            { route: 'error', name: 'error', title: 'Error', moduleId: 'views/error', nav: false },
            { route: 'notfound', name: 'notfound', title: 'Not Found', moduleId: 'views/notfound', nav: false },
            { route: 'unauthorized', name: 'unauthorized', title: 'Unauthorized', moduleId: 'views/unauthorized', nav: false },
            // Teams
            { route: 'teams', name: 'teams', title: 'Teams', moduleId: 'views/teams/index', nav: true },
            { route: 'teams/create', name: 'teamCreate', title: 'Create Team', moduleId: 'views/teams/edit', nav: false, auth: 'Contributor' },
            { route: 'teams/:id/edit', name: 'teamEdit', title: 'Edit Team', moduleId: 'views/teams/edit', nav: false, auth: 'Contributor' },
            { route: 'teams/:id/details', name: 'teamDetails', title: 'Team Details', moduleId: 'views/teams/detail', nav: false },
            { route: 'teams/import', name: 'teamImport', title: 'Import', moduleId: 'views/teams/import', nav: true, auth: 'Administrator' },
            // Admin
            { route: 'admin', name: 'admin', title: 'Admin', moduleId: 'views/admin/index', nav: true, auth: 'Administrator' },
            // Account
            { route: 'account/:id?', name: 'account', title: 'Account', moduleId: 'views/account/index', nav: false, auth: false },
            { route: 'account/:id/edit', name: 'accountEdit', title: 'Account', moduleId: 'views/account/edit', nav: false, auth: true },
            // Boring
            { route: 'privacy', name: 'privacy', title: 'Privacy Policy', moduleId: 'views/boring/privacy', nav: false, auth: false },
            { route: 'tos', name: 'tos', title: 'Terms of Service', moduleId: 'views/boring/tos', nav: false, auth: false },
        ]);
        config.mapUnknownRoutes({ route: 'notfound', moduleId: 'views/notfound' });
    }

    private navToggled: boolean;

    toggleDropdown(): void {
        this.navToggled = !this.navToggled;
    }

    get topnavCss() {
        return 'topnav' + (this.navToggled ? ' responsive' : '');
    }

    @computedFrom('router.navigation', 'accountService.userProfile')
    get authorizedRoutes() {
        return this.router.navigation.filter(n => {
            return this.accountService.isInRoles(n.config['auth']);
        });
    }
}
