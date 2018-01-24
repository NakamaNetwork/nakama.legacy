import { autoinject, bindable, customElement } from 'aurelia-framework';
import { UnitRole } from '../models/imported';
import { NumberHelper } from '../tools/number-helper';

@autoinject
@customElement('unit-role-picker')
export class UnitRolePicker {

    @bindable
    unitRole = 0;

    unitRoles = NumberHelper.getPairs(UnitRole, true);

    maximum = this.unitRoles.map(x => x.id).reduce((acc, val) => acc += val);
}