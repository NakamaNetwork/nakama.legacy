import { autoinject } from 'aurelia-framework';
import { RouterConfiguration, Router } from 'aurelia-router';
import { AccountService } from './services/account-service';

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
        //config.addPipelineStep('authorize', AuthorizeStep);
        config.map([
            { route: ['', '/', 'home'], name: 'home', title: 'Home', moduleId: 'views/index', nav: true },
            { route: 'error', name: 'error', title: 'Error', moduleId: 'views/error', nav: false },
            { route: 'notfound', name: 'notfound', title: 'Not Found', moduleId: 'views/notfound', nav: false },
            // Stages
            { route: 'stages', name: 'stages', title: 'Stages', moduleId: 'views/stages/index', nav: true },
            { route: 'stages/create', name: 'stageCreate', title: 'Create Stage', moduleId: 'views/stages/edit', nav: false },
            { route: 'stages/:id/edit', name: 'stageEdit', title: 'Edit Stage', moduleId: 'views/stages/edit', nav: false },
            { route: 'stages/:id/details', name: 'stageDetails', title: 'Stage Details', moduleId: 'views/stages/detail', nav: false },
            // Teams
            { route: 'teams', name: 'teams', title: 'Teams', moduleId: 'views/teams/index', nav: true },
            { route: 'teams/create', name: 'teamCreate', title: 'Create Team', moduleId: 'views/teams/edit', nav: false },
            { route: 'teams/:id/edit', name: 'teamEdit', title: 'Edit Team', moduleId: 'views/teams/edit', nav: false },
            { route: 'teams/:id/details', name: 'teamDetails', title: 'Team Details', moduleId: 'views/teams/detail', nav: false },
            // Admin
            { route: 'admin', name: 'admin', title: 'Admin', moduleId: 'views/admin/index', nav: true },
            // Account
            { route: 'account', name: 'account', title: 'Account', moduleId: 'views/account/index', nav: false },
            { route: 'account/login', name: 'login', title: 'Login', moduleId: 'views/account/login', nav: false },
            { route: 'account/register', name: 'register', title: 'Register', moduleId: 'views/account/register', nav: false }
        ]);
        config.mapUnknownRoutes({ route: 'notfound', moduleId: 'views/notfound' });
    }
}
