import { bindable, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { StageType } from '../models/imported';

@autoinject
@customElement('stage-type-picker')
export class StageTypePicker {
    private element: Element;

    @bindable
    stageType = 0;

    constructor(element: Element) {
        this.element = element;
    }

    stageTypes = [{
        id: undefined,
        name: 'Any'
    }].concat(Object.keys(StageType).map((k) => {
        return { id: StageType[k], name: k };
    }).filter(x => !Number.isNaN(Number(x.id))).filter(x => x.id !== 0));
}