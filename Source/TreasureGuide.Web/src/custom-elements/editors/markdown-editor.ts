import { autoinject, bindable, customElement } from 'aurelia-framework';

@autoinject
@customElement('markdown-editor')
export class MarkdownEditor {

    @bindable value;
    @bindable placeholder: string;
    @bindable editing: boolean = true;

    toggle() {
        this.editing = !this.editing;
    }
}