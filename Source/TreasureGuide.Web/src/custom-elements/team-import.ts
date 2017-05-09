import { computedFrom } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { CalcParser } from '../tools/calc-parser';
import { DialogController } from 'aurelia-dialog';

@autoinject
export class TeamImportView {
    private controller: DialogController;
    private calcParser: CalcParser;
    
    input = '';

    constructor(calcParser: CalcParser, controller: DialogController) {
        this.controller = controller;
        this.calcParser = calcParser;
    }

    @computedFrom('input')
    get import() {
        var ids = this.calcParser.parse(this.input);
        return this.calcParser.convert(ids);
    }

    submit() {
        this.controller.ok(this.import);
    };

    cancel() {
        this.controller.cancel();
    };
}