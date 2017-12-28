import { autoinject, computedFrom } from 'aurelia-framework';
import { RouterConfiguration, Router } from 'aurelia-router';
import { AccountService } from './services/account-service';
import { AuthorizeStep } from './tools/authorize-step';

@autoinject
export class App {
    public router: Router;
    public message = 'Shukko da!';
    public accountService: AccountService;

    constructor(accountService: AccountService) {
        this.accountService = accountService;
    }

    configureRouter(config: RouterConfiguration, router: Router): void {
        this.router = router;
        config.title = 'NakamaDB';
        config.addAuthorizeStep(AuthorizeStep);
        config.map([
            { route: ['', '/', 'home'], name: 'home', title: 'Home', moduleId: 'views/index', nav: true },
            { route: 'error', name: 'error', title: 'Error', moduleId: 'views/error', nav: false },
            { route: 'notfound', name: 'notfound', title: 'Not Found', moduleId: 'views/notfound', nav: false },
            { route: 'unauthorized', name: 'unauthorized', title: 'Unauthorized', moduleId: 'views/unauthorized', nav: false },
            // Stages
            { route: 'stages', name: 'stages', title: 'Stages', moduleId: 'views/stages/index', nav: true },
            { route: 'stages/create', name: 'stageCreate', title: 'Create Stage', moduleId: 'views/stages/edit', nav: false, auth: 'Administrator' },
            { route: 'stages/:id/edit', name: 'stageEdit', title: 'Edit Stage', moduleId: 'views/stages/edit', nav: false, auth: 'Administrator' },
            { route: 'stages/:id/details', name: 'stageDetails', title: 'Stage Details', moduleId: 'views/stages/detail', nav: false, auth: 'Administrator' },
            // Teams
            { route: 'teams', name: 'teams', title: 'Teams', moduleId: 'views/teams/index', nav: true },
            { route: 'teams/create', name: 'teamCreate', title: 'Create Team', moduleId: 'views/teams/edit', nav: false, auth: 'Contributor' },
            { route: 'teams/:id/edit', name: 'teamEdit', title: 'Edit Team', moduleId: 'views/teams/edit', nav: false, auth: 'Contributor' },
            { route: 'teams/:id/details', name: 'teamDetails', title: 'Team Details', moduleId: 'views/teams/detail', nav: false },
            // Admin
            { route: 'admin', name: 'admin', title: 'Admin', moduleId: 'views/admin/index', nav: true, auth: 'Administrator' },
            // Account
            { route: 'account', name: 'account', title: 'Account', moduleId: 'views/account/index', nav: false, auth: true }
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
