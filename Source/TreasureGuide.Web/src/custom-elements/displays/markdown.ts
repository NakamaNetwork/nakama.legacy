import { bindable, computedFrom, customElement } from 'aurelia-framework';
import * as marked from 'marked';

@customElement('markdown')
export class Markdown {
    @bindable value: string;

    @computedFrom('value')
    get html() {
        return marked.parse(this.value || '');
    }
}