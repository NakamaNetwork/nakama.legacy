import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { NumberHelper } from '../../tools/number-helper';

@autoinject
@customElement('bit-button')
export class BitButton {
    @bindable value: number = 0;
    @bindable model: number = 0;
    @bindable title: string;

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
            this.value -= this.model;
        } else {
            this.value |= this.model;
        }
    }
}

export class BitMaxModel {
    public maximum: number;
    public maxOrder: number[];
    private enumType;

    constructor(maximum: number, value: number, enumType) {
        this.maximum = NumberHelper.forceNumber(maximum);
        this.enumType = enumType;
        this.maxOrder = NumberHelper.splitEnum(value, this.enumType).map(x => x.id);
    }

    verify(newValue: number, oldValue: number): Promise<number> {
        return new Promise<number>((resolve, reject) => {
            if (this.maximum < 1) {
                reject();
            }
            var newValues = newValue ? NumberHelper.splitEnum(newValue, this.enumType).map(x => x.id) : [];

            newValues.filter(x => this.maxOrder.indexOf(x) === -1).forEach(x => this.maxOrder.push(x));
            this.maxOrder = this.maxOrder.filter(x => newValues.indexOf(x) > -1);

            if (newValues.length > this.maximum) {
                var removed = this.maxOrder.shift();
                var adjusted = NumberHelper.combine(newValues.filter(x => x !== removed));
                if (adjusted !== newValue) {
                    resolve(adjusted);
                }
            }
            reject();
        });
    }
}