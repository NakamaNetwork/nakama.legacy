import { bindable, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { UnitClass } from '../models/imported';
import { NumberHelper } from '../tools/number-helper';

@autoinject
@customElement('unit-class-picker')
export class UnitClassPicker {
    @bindable unitClass: UnitClass = 0;

    unitClasses = NumberHelper.getPairs(UnitClass, true);

    maximum = this.unitClasses.map(x => x.id).reduce((acc, val) => acc += val);
}