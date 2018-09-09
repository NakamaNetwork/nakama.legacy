import { Aurelia, Container } from 'aurelia-framework'
import environment from './environment';
import * as marked from 'marked';
import { CustomMarkedRenderer } from './tools/custom-marked-renderer';

export function configure(aurelia: Aurelia) {
    aurelia.use
        .standardConfiguration();

    if (environment.debug) {
        aurelia.use.developmentLogging();
    }

    if (environment.testing) {
        aurelia.use.plugin('aurelia-testing');
    }

    aurelia.use
        .plugin('aurelia-fetch-client')
        .plugin('aurelia-dialog')
        .plugin('aurelia-validation')
        .plugin('aurelia-plugins-pagination')
        // Custom Elements
        .feature('./custom-attributes')
        .feature('./custom-elements')
        .feature('./custom-elements/comments')
        .feature('./custom-elements/dialogs')
        .feature('./custom-elements/displays')
        .feature('./custom-elements/editors')
        .feature('./value-converters');

    marked.setOptions({
        renderer: new CustomMarkedRenderer(),
        sanitize: true
    });

    aurelia.start().then(() => aurelia.setRoot());
}