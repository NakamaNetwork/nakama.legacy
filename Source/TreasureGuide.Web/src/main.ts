import { Aurelia } from 'aurelia-framework'
import environment from './environment';

export function configure(aurelia: Aurelia) {
    aurelia.use
        .standardConfiguration()
        .feature('resources');

    if (environment.debug) {
        aurelia.use.developmentLogging();
    }

    if (environment.testing) {
        aurelia.use.plugin('aurelia-testing');
    }

    aurelia.use.plugin('aurelia-fetch-client')
        .plugin('aurelia-materialize-bridge', b => b.useAll())
        .globalResources([
            'materialize-css/css/materialize.css',
            './css/site.css'
        ]);

    aurelia.start().then(() => aurelia.setRoot());
}
