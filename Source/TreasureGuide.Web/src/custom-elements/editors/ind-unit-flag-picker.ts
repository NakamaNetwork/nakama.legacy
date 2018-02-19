import { autoinject, bindable, customElement } from 'aurelia-framework';
import { NameIdPair } from '../../models/name-id-pair';
import { BitMaxModel } from './bit-button';
import { NumberHelper } from '../../tools/number-helper';
import { IndividualUnitFlags } from '../../models/imported';

@autoinject
@customElement('ind-unit-flag-picker')
export class IndUnitFlagPicker {
    private element: Element;

    @bindable
    value: number;
    @bindable
    maximum: number;

    protected values: NameIdPair[];
    protected maxModel: BitMaxModel;

    constructor(element: Element) {
        this.values = NumberHelper.getPairs(IndividualUnitFlags, true);
        this.element = element;
    }

    bind() {
        this.maxModel = new BitMaxModel(this.maximum, this.value, IndividualUnitFlags);
    }

    maximumChanged(newValue, oldValue) {
        this.maxModel.maximum = newValue;
    }

    valueChanged(newValue, oldValue) {
        this.maxModel.verify(newValue, oldValue).then(x => {
            this.value = x;
        }, x => {
            if (newValue !== oldValue && oldValue != null) {
                var bubble = new CustomEvent('changed',
                    {
                        detail: { newValue: newValue, oldValue: oldValue, viewModel: this },
                        bubbles: true
                    });
                this.element.dispatchEvent(bubble);
            }
        });
    }
}