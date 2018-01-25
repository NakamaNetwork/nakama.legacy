import { autoinject, bindable, customElement } from 'aurelia-framework';
import { UnitRole } from '../../models/imported';
import { NameIdPair } from '../../models/name-id-pair';
import { BitMaxModel } from './bit-button';
import { NumberHelper } from '../../tools/number-helper';

@autoinject
@customElement('unit-role-picker')
export class UnitRolePicker {
    @bindable value: number;
    @bindable maximum: number;
    protected values: NameIdPair[];
    protected maxModel: BitMaxModel;

    private silent: boolean;

    constructor() {
        this.values = NumberHelper.getPairs(UnitRole, true);
    }

    bind() {
        this.maxModel = new BitMaxModel(this.maximum, this.value, UnitRole);
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