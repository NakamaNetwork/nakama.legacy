import { bindable, customElement } from 'aurelia-framework';

@customElement('global-display')
export class GlobalDisplay {
    @bindable global = false;
}