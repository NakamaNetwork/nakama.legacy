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
            { route: 'notfound', name: 'notfound', moduleId: 'views/notfound', nav: false }
        ]);
        config.mapUnknownRoutes({ route: 'notfound', moduleId: 'views/notfound' });
    }
}
