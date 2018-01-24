import { autoinject, bindable, customElement } from 'aurelia-framework';
import { UnitType } from '../models/imported';
import { NumberHelper } from '../tools/number-helper';

@autoinject
@customElement('unit-type-picker')
export class UnitTypePicker {

    @bindable
    unitType = 0;

    unitTypes = NumberHelper.getPairs(UnitType, false);

    maximum = this.unitTypes.map(x => x.id).reduce((acc, val) => acc += val);
}