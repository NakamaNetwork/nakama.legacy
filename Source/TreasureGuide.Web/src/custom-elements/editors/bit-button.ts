import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';

@autoinject
@customElement('bit-button')
export class BitButton {
    @bindable value: number = 0;
    @bindable model: number = 0;
    @bindable title: string;
    @bindable exclusive: boolean = false;

    @computedFrom('value', 'model')
    get toggled() {
        if (!this.value) {
            this.value = 0;
        }
        if (!this.model) {
            this.model = 0;
        }
        return (this.value & this.model) === this.model;
    }

    @computedFrom('toggled')
    get toggleClass() {
        return this.toggled ? 'on' : 'off';
    }

    toggle() {
        if (this.toggled) {
            if (this.exclusive) {
                this.value = 0;
            } else {
                this.value -= this.model;
            }
        } else {
            if (this.exclusive) {
                this.value = this.model;
            } else {
                this.value |= this.model;
            }
        }
    }
}