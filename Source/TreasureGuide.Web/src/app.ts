import { autoinject, computedFrom } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import { RouterConfiguration, Router } from 'aurelia-router';
import { AccountService } from './services/account-service';
import { AuthorizeStep } from './tools/authorize-step';
import { NewsService } from './services/news-service';
import { BoxService } from './services/box-service';
import { RoleConstants } from './models/imported';
import { ThiccStep } from './tools/thicc-step';
import { NotificationQueryService } from './services/query/notification-query-service';

@autoinject
export class App {
    public router: Router;
    public accountService: AccountService;
    private boxService: BoxService;
    private notificationService: NotificationQueryService;
    private ea: EventAggregator;
    private newsService: NewsService;

    constructor(accountService: AccountService, eventAggregator: EventAggregator, newsService: NewsService,
        boxService: BoxService, notificationService: NotificationQueryService) {
        this.accountService = accountService;
        this.newsService = newsService;
        this.boxService = boxService;
        this.notificationService = notificationService;
        this.ea = eventAggregator;
    }

    activate() {
        this.newsService.show();
        window.addEventListener('beforeunload', (e) => {
            if (this.boxService.currentBox && this.boxService.currentBox.dirty) {
                this.boxService.save();
                e.returnValue = true;
            }
        });
        this.ea.subscribe('router:navigation:processing', response => {
            this.boxService.save();
        });
        this.ea.subscribe('router:navigation:complete', response => {
            this.navToggled = false;
            if (this.router) {
                ThiccStep.cleanForce(this.router.currentInstruction);
            }
        });
        setInterval(() => {
            this.refreshNotifications();
        }, 30000);
        this.refreshNotifications();
    }

    configureRouter(config: RouterConfiguration, router: Router): void {
        this.router = router;
        config.title = 'Nakama Network';
        config.addAuthorizeStep(AuthorizeStep);
        config.addPreRenderStep(ThiccStep);
        config.options.pushState = true;
        config.options.root = '/';
        config.map([
            { route: ['', '/', '_=_', 'home'], name: 'home', title: 'Home', moduleId: 'views/index', nav: false },
            { route: 'news', name: 'news', title: 'News', moduleId: 'views/boring/news', nav: true },
            { route: 'error', name: 'error', title: 'Error', moduleId: 'views/error', nav: false },
            { route: 'notfound', name: 'notfound', title: 'Not Found', moduleId: 'views/notfound', nav: false },
            { route: 'unauthorized', name: 'unauthorized', title: 'Unauthorized', moduleId: 'views/unauthorized', nav: false },
            // Teams
            { route: 'teams', name: 'teams', title: 'Teams', moduleId: 'views/teams/index', nav: true },
            { route: 'teams/create', name: 'teamCreate', title: 'Create Team', moduleId: 'views/teams/edit', nav: false, auth: RoleConstants.Contributor },
            { route: 'teams/:id/edit', name: 'teamEdit', title: 'Edit Team', moduleId: 'views/teams/edit', nav: false, auth: RoleConstants.Contributor },
            { route: 'teams/:id/details', name: 'teamDetails', title: 'Team Details', moduleId: 'views/teams/detail', nav: false },
            { route: 'teams/import', name: 'teamImport', title: 'Bulk', moduleId: 'views/teams/import', nav: true, thicc: true, auth: RoleConstants.GCRViewer },
            // Stages
            { route: 'stages', name: 'stages', title: 'Stages', moduleId: 'views/stages/index', nav: true },
            { route: 'stages/:id/details', name: 'stageDetails', title: 'Stage Details', moduleId: 'views/stages/detail', nav: false },
            // Boxes
            { route: 'boxes', name: 'boxes', title: 'Boxes', moduleId: 'views/boxes/index', nav: false, auth: RoleConstants.BoxUser },
            { route: 'boxes/create', name: 'boxCreate', title: 'Create Box', moduleId: 'views/boxes/edit', nav: false, auth: RoleConstants.BoxUser },
            { route: 'boxes/:id/edit', name: 'boxEdit', title: 'Edit Box', moduleId: 'views/boxes/edit', nav: false, auth: RoleConstants.BoxUser },
            { route: 'boxes/:id/details', name: 'boxDetails', title: 'Box Details', moduleId: 'views/boxes/detail', nav: false },
            // Notifications
            { route: 'notifications', name: 'notifications', title: 'Notifications', moduleId: 'views/notifications/index', nav: false, auth: true },
            // GCR
            { route: 'gcr/edit', name: 'gcrEdit', title: 'Edit GCR', moduleId: 'views/gcr/edit', nav: false, auth: RoleConstants.GCRAdmin },
            // Admin
            { route: 'admin', name: 'admin', title: 'Admin', moduleId: 'views/admin/index', nav: true, auth: RoleConstants.Administrator },
            { route: 'commentAdmin', name: 'commentAdmin', title: 'Comments', moduleId: 'views/admin/comments', nav: false, auth: RoleConstants.Administrator },
            // Account
            { route: ['account/:id?', 'profile/:id?'], name: 'profile', title: 'Account', moduleId: 'views/profile/index', nav: false },
            { route: ['account/:id/edit', 'profile/:id/edit'], name: 'profileEdit', title: 'Account', moduleId: 'views/profile/edit', nav: false, auth: true },
            // Boring
            { route: 'privacy', name: 'privacy', title: 'Privacy Policy', moduleId: 'views/boring/privacy', nav: false },
            { route: 'tos', name: 'tos', title: 'Terms of Service', moduleId: 'views/boring/tos', nav: false },
            { route: 'about', name: 'about', title: 'About', moduleId: 'views/boring/intro', nav: true },
            { route: 'markdown', name: 'markdown', title: 'Markdown', moduleId: 'views/boring/markdown', nav: false },
            // Give me your money
            { route: 'support', name: 'support', title: 'Credits', moduleId: 'views/support/index', nav: true },
            { route: 'donate', name: 'donate', title: 'Donate', moduleId: 'views/donate/index', nav: true },
            { route: 'donate/update', name: 'donationUpdate', title: 'Updating Donations', moduleId: 'views/donate/update', nav: false },
            { route: 'donate/cancel', name: 'donationCancelled', title: 'Donation Cancelled', moduleId: 'views/donate/cancel', nav: false },
            { route: 'donate/history', name: 'donationHistory', title: 'Donation History', moduleId: 'views/donate/history', nav: false, auth: true },
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
        return this.fragment === '/profile' ? 'active' : '';
    }

    @computedFrom('boxService', 'boxService.currentBox')
    get hasBox() {
        return this.boxService && this.boxService.currentBox;
    }

    @computedFrom('fragment', 'hasBox')
    get boxClass() {
        return (this.fragment === '/boxes' ? 'active' : '') + ' ' + (this.hasBox ? '' : '_danger');
    }

    @computedFrom('hasBox')
    get boxTitle() {
        return this.hasBox ? 'Current Box: "' + this.boxService.currentBox.name + '"' : 'Select a Box';
    }

    @computedFrom('router.navigation', 'accountService.userProfile')
    get authorizedRoutes() {
        return this.router.navigation.filter(n => {
            return this.accountService.isInRoles(n.config['auth']);
        });
    }

    notificationCount: number;

    @computedFrom('notificationCount')
    get notificationTitle() {
        return this.notificationCount + ' unread notification' + (this.notificationCount != 1 ? 's' : '');
    }

    @computedFrom('notificationCount')
    get hasNotifications() {
        return this.notificationCount > 0;
    }

    @computedFrom('notificationCount')
    get notificationClass() {
        return this.notificationCount > 0 ? 'fa-envelope' : 'fa-envelope-open';
    }

    refreshNotifications() {
        this.notificationService.count().then(x => {
            this.notificationCount = x;
        });
    }
}
