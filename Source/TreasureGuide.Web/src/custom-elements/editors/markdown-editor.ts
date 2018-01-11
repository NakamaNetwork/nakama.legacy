import { bindable, computedFrom, customElement } from 'aurelia-framework';

@customElement('markdown-editor')
export class MarkdownEditor {
    @bindable value;
    @bindable placeholder: string;
    @bindable editing: boolean = true;

    toggle() {
        this.editing = !this.editing;
    }
}