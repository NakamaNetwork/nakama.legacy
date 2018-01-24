import { autoinject, bindable, customElement } from 'aurelia-framework';
import { UnitRole } from '../models/imported';
import { StringHelper } from '../tools/string-helper';

@autoinject
@customElement('unit-role-picker')
export class UnitRolePicker {

    @bindable
    unitRole = 0;

    unitRoles = Object.keys(UnitRole).map((k) => {
        return { id: UnitRole[k], name: StringHelper.prettifyEnum(k) };
    }).filter(x => !Number.isNaN(Number(x.id))).filter(x => x.id !== 0);

    maximum = this.unitRoles.map(x => x.id).reduce((acc, val) => acc += val);
}