import { Aurelia } from 'aurelia-framework'
import environment from './environment';
import * as marked from 'marked';

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

export class CustomMarkedRenderer extends marked.Renderer {
    public image(href: string, title: string, text: string): string {
        var linkTitle = title || text || 'image';
        return '<a href="' + href + '" target="_blank"><i class="fa fa-fw fa-file-image-o"></i>' + linkTitle + '</a>';
    }
}