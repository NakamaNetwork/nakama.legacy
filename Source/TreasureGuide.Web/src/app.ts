import { autoinject } from 'aurelia-dependency-injection';
import { RouterConfiguration, Router } from 'aurelia-router';

export class App {
    router: Router;
    message = 'Shukko da!';

    configureRouter(config: RouterConfiguration, router: Router): void {
        this.router = router;
        config.title = 'Nakama';
        //config.addPipelineStep('authorize', AuthorizeStep);
        config.map([
            { route: ['', '/', 'home'], name: 'home', title: 'Home', moduleId: 'views/index', nav: true },
            { route: 'error', name: 'error', moduleId: 'views/error', nav: false },
            { route: 'notfound', name: 'notfound', moduleId: 'views/notfound', nav: false },
            // Units
            { route: 'units', name: 'units', title: 'Units', moduleId: 'views/units/index', nav: true },
            { route: 'stages', name: 'stages', title: 'Stages', moduleId: 'views/stages/index', nav: true },
            { route: 'stages/create', name: 'stageCreate', title: 'Create Stage', moduleId: 'views/stages/edit', nav: false },
            { route: 'stages/:id/edit', name: 'stageEdit', title: 'Edit Stage', moduleId: 'views/stages/edit', nav: false },
            { route: 'teams', name: 'teams', title: 'Teams', moduleId: 'views/teams/index', nav: true },
            { route: 'teams/create', name: 'teamCreate', title: 'Create Team', moduleId: 'views/teams/edit', nav: false },
            { route: 'teams/:id/edit', name: 'teamEdit', title: 'Edit Team', moduleId: 'views/teams/edit', nav: false },
        ]);
        config.mapUnknownRoutes({ route: 'notfound', moduleId: 'views/notfound' });
    }
}
