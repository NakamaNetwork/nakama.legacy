import { bindable, observable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { CalcParser } from '../tools/calc-parser';

@customElement('team-import')
@autoinject
export class TeamImportView {
    private element: Element;
    private calcParser: CalcParser;

    @bindable @observable input = '';

    constructor(element: Element, calcParser: CalcParser) {
        this.element = element;
        this.calcParser = calcParser;
    }

    @computedFrom('input')
    get import() {
        return this.calcParser.parse(this.input);
    }
}