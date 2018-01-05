import { bindable, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { UnitClass } from '../models/imported';

@autoinject
@customElement('unit-class-picker')
export class UnitDisplay {
    private element: Element;

    @bindable unitClass = 0;

    constructor(element: Element) {
        this.element = element;
    }

    unitClasses = Object.keys(UnitClass).map((k) => {
        return { id: UnitClass[k], name: k };
    }).filter(x => !Number.isNaN(Number(x.id))).filter(x => x.id !== 0);
}