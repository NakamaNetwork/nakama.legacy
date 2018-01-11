import { Aurelia } from 'aurelia-framework'
import environment from './environment';
import * as showdown from 'showdown';

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
        .plugin('aurelia-markdown')
        // Custom Elements
        .feature('./custom-attributes')
        .feature('./custom-elements')
        .feature('./custom-elements/dialogs')
        .feature('./custom-elements/displays')
        .feature('./custom-elements/editors')
        .feature('./value-converters');

    showdown.setOption('tables', true);
    showdown.setOption('strikethrough', true);
    showdown.setOption('emoji', true);
    showdown.setOption('emoji', true);

    aurelia.start().then(() => aurelia.setRoot());
}
