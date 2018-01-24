import { autoinject, bindable, customElement } from 'aurelia-framework';
import { UnitType } from '../models/imported';

@autoinject
@customElement('unit-type-picker')
export class UnitTypePicker {

    @bindable
    unitType = 0;

    unitTypes = Object.keys(UnitType).map((k) => {
        return { id: UnitType[k], name: k };
    }).filter(x => !Number.isNaN(Number(x.id))).filter(x => x.id !== 0);

    maximum = this.unitTypes.map(x => x.id).reduce((acc, val) => acc += val);
}