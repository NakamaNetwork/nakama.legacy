import { bindable, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { UnitType } from '../models/imported';
import { StringHelper } from '../tools/string-helper';

@autoinject
@customElement('unit-type-picker')
export class UnitTypePicker {
    private element: Element;

    @bindable
    unitType = 0;

    constructor(element: Element) {
        this.element = element;
    }

    unitTypes = [{
        id: undefined,
        name: 'Any'
    }].concat(Object.keys(UnitType).map((k) => {
        return { id: UnitType[k], name: StringHelper.prettifyEnum(k) };
    }).filter(x => !Number.isNaN(Number(x.id))).filter(x => x.id !== 0));
}