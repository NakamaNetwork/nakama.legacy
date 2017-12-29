import { bindable, computedFrom, customElement } from 'aurelia-framework';

@customElement('global-display')
export class GlobalDisplay {
    @bindable global = false;

    @computedFrom('global')
    get iconClass() {
        return 'fa fa-fw ' + (this.global ? 'fa-globe' : '');
    }
}