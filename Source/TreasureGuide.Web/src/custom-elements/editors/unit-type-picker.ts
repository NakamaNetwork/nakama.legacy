import { autoinject, bindable, customElement } from 'aurelia-framework';
import { UnitType } from '../../models/imported';
import { NameIdPair } from '../../models/name-id-pair';
import { BitMaxModel } from './bit-button';
import { NumberHelper } from '../../tools/number-helper';

@autoinject
@customElement('unit-type-picker')
export class UnitTypePicker {
    @bindable
    value: number;
    @bindable
    maximum: number;
    protected values: NameIdPair[];
    protected maxModel: BitMaxModel;

    constructor() {
        this.values = NumberHelper.getPairs(UnitType, false);
    }

    bind() {
        this.maxModel = new BitMaxModel(this.maximum, this.value, UnitType);
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