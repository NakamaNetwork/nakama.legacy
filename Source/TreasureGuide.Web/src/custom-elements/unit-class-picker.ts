import { bindable, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { UnitClass } from '../models/imported';
import { StringHelper } from '../tools/string-helper';

@autoinject
@customElement('unit-class-picker')
export class UnitClassPicker {
    private element: Element;

    @bindable unitClass = 0;

    constructor(element: Element) {
        this.element = element;
    }

    unitClasses = Object.keys(UnitClass).map((k) => {
        return { id: UnitClass[k], name: StringHelper.prettifyEnum(k) };
    }).filter(x => !Number.isNaN(Number(x.id))).filter(x => x.id !== 0);
}