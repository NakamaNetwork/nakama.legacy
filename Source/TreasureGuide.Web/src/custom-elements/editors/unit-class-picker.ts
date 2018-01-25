import { autoinject, bindable, customElement } from 'aurelia-framework';
import { UnitClass } from '../../models/imported';
import { NameIdPair } from '../../models/name-id-pair';
import { BitMaxModel } from './bit-button';
import { NumberHelper } from '../../tools/number-helper';

@autoinject
@customElement('unit-class-picker')
export class UnitClassPicker {
    @bindable value: number;
    @bindable maximum: number;
    protected values: NameIdPair[];
    protected maxModel: BitMaxModel;

    constructor() {
        this.values = NumberHelper.getPairs(UnitClass, true);
    }

    bind() {
        this.maxModel = new BitMaxModel(this.maximum, this.value, UnitClass);
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