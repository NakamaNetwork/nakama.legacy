import { autoinject, bindable, customElement } from 'aurelia-framework';
import { NameIdPair } from '../../models/name-id-pair';
import { BitMaxModel } from './bit-button';
import { NumberHelper } from '../../tools/number-helper';
import { IndividualUnitFlags } from '../../models/imported';

@autoinject
@customElement('ind-unit-flag-picker')
export class IndUnitFlagPicker {
    @bindable
    value: number;
    @bindable
    maximum: number;
    protected values: NameIdPair[];
    protected maxModel: BitMaxModel;

    constructor() {
        this.values = NumberHelper.getPairs(IndividualUnitFlags, true);
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
            // ...
        });
    }
}