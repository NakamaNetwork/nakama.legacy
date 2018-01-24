import { bindable, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { UnitClass } from '../models/imported';
import { StringHelper } from '../tools/string-helper';

@autoinject
@customElement('unit-class-picker')
export class UnitClassPicker {
    @bindable unitClass: UnitClass = 0;

    unitClasses = Object.keys(UnitClass).map((k) => {
        return { id: UnitClass[k], name: StringHelper.prettifyEnum(k) };
    }).filter(x => !Number.isNaN(Number(x.id))).filter(x => x.id !== 0);

    maximum = this.unitClasses.map(x => x.id).reduce((acc, val) => acc += val);
}