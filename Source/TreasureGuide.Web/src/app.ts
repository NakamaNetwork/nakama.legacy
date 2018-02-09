import { autoinject, computedFrom } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import { RouterConfiguration, Router } from 'aurelia-router';
import { AccountService } from './services/account-service';
import { AuthorizeStep } from './tools/authorize-step';
import { NewsService } from './services/news-service';
import { BoxService } from './services/box-service';

@autoinject
export class App {
    public router: Router;
    public message = 'Shukko da!';
    public accountService: AccountService;
    private boxService: BoxService;
    private ea: EventAggregator;
    private newsService: NewsService;

    constructor(accountService: AccountService, eventAggregator: EventAggregator, newsService: NewsService, boxService: BoxService) {
        this.accountService = accountService;
        this.ea = eventAggregator;
        this.ea.subscribe('router:navigation:complete', response => {
            this.navToggled = false;
        });
        this.newsService = newsService;
        this.boxService = boxService;
    }

    activate() {
        this.newsService.show();
    }

    configureRouter(config: RouterConfiguration, router: Router): void {
        this.router = router;
        config.title = 'Nakama Network';
        config.addAuthorizeStep(AuthorizeStep);
        config.options.pushState = true;
        config.map([
            { route: ['', '/', '_=_', 'home'], name: 'home', title: 'Home', moduleId: 'views/index', nav: false },
            { route: 'news', name: 'news', title: 'News', moduleId: 'views/boring/news', nav: true },
            { route: 'error', name: 'error', title: 'Error', moduleId: 'views/error', nav: false },
            { route: 'notfound', name: 'notfound', title: 'Not Found', moduleId: 'views/notfound', nav: false },
            { route: 'unauthorized', name: 'unauthorized', title: 'Unauthorized', moduleId: 'views/unauthorized', nav: false },
            // Teams
            { route: 'teams', name: 'teams', title: 'Teams', moduleId: 'views/teams/index', nav: true },
            { route: 'teams/create', name: 'teamCreate', title: 'Create Team', moduleId: 'views/teams/edit', nav: false, auth: 'Contributor' },
            { route: 'teams/:id/edit', name: 'teamEdit', title: 'Edit Team', moduleId: 'views/teams/edit', nav: false, auth: 'Contributor' },
            { route: 'teams/:id/details', name: 'teamDetails', title: 'Team Details', moduleId: 'views/teams/detail', nav: false },
            { route: 'teams/import', name: 'teamImport', title: 'Import', moduleId: 'views/teams/import', nav: true, auth: 'Administrator' },
            // Stages
            { route: 'stages', name: 'stages', title: 'Stages', moduleId: 'views/stages/index', nav: true },
            { route: 'stages/:id/details', name: 'stageDetails', title: 'Stage Details', moduleId: 'views/stages/detail', nav: false },
            // Boxes
            { route: 'boxes', name: 'boxes', title: 'Boxes', moduleId: 'views/boxes/index', nav: false },
            { route: 'boxes/create', name: 'boxCreate', title: 'Create Box', moduleId: 'views/boxes/edit', nav: false, auth: 'Contributor' },
            { route: 'boxes/:id/edit', name: 'boxEdit', title: 'Edit Box', moduleId: 'views/boxes/edit', nav: false, auth: 'Contributor' },
            { route: 'boxes/:id/details', name: 'boxDetails', title: 'Box Details', moduleId: 'views/boxes/detail', nav: false },
            // Admin
            { route: 'admin', name: 'admin', title: 'Admin', moduleId: 'views/admin/index', nav: true, auth: 'Administrator' },
            // Account
            { route: 'account/:id?', name: 'account', title: 'Account', moduleId: 'views/account/index', nav: false, auth: false },
            { route: 'account/:id/edit', name: 'accountEdit', title: 'Account', moduleId: 'views/account/edit', nav: false, auth: true },
            // Boring
            { route: 'privacy', name: 'privacy', title: 'Privacy Policy', moduleId: 'views/boring/privacy', nav: false, auth: false },
            { route: 'tos', name: 'tos', title: 'Terms of Service', moduleId: 'views/boring/tos', nav: false, auth: false },
            { route: 'about', name: 'about', title: 'About', moduleId: 'views/boring/intro', nav: true },
            { route: 'markdown', name: 'markdown', title: 'Markdown', moduleId: 'views/boring/markdown', nav: false },
        ]);
        config.mapUnknownRoutes({ route: 'notfound', moduleId: 'views/notfound' });
    }

    private navToggled: boolean;

    toggleDropdown(): void {
        this.navToggled = !this.navToggled;
    }

    @computedFrom('navToggled')
    get topnavCss() {
        return 'topnav' + (this.navToggled ? ' responsive' : '');
    }

    @computedFrom('router', 'router.currentInstruction', 'router.currentInstruction.fragment')
    get fragment() {
        if (this.router && this.router.currentInstruction) {
            return this.router.currentInstruction.fragment;
        }
        return '';
    }

    @computedFrom('fragment')
    get accountIsActive() {
        return this.fragment === '/account' ? 'active' : '';
    }

    @computedFrom('fragment')
    get boxIsActive() {
        return this.fragment === '/boxes' ? 'active' : '';
    }

    @computedFrom('boxService', 'boxService.currentBox')
    get boxIcon() {
        return (this.boxService && this.boxService.currentBox) ? 'fa-address-book' : 'fa-address-book-o';
    }

    @computedFrom('boxService', 'boxService.currentBox')
    get boxTitle() {
        return (this.boxService && this.boxService.currentBox) ? this.boxService.currentBox.name : 'Select a Box';
    }

    @computedFrom('router.navigation', 'accountService.userProfile')
    get authorizedRoutes() {
        return this.router.navigation.filter(n => {
            return this.accountService.isInRoles(n.config['auth']);
        });
    }
}
