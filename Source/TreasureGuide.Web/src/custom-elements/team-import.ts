import { bindable } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { CalcParser } from '../tools/calc-parser';
import { DialogController } from 'aurelia-dialog';

@autoinject
export class TeamImportView {
    private controller: DialogController;
    private calcParser: CalcParser;

    @bindable input = '';

    team = new Array(6);
    ship = 1;

    constructor(calcParser: CalcParser, controller: DialogController) {
        this.controller = controller;
        this.calcParser = calcParser;
    }

    inputChanged(newValue: string, oldValue: string) {
        var teamIds = this.calcParser.parse(this.input);
        this.team = this.calcParser.convert(teamIds.units);
        this.ship = teamIds.ship;
    }

    submit() {
        this.controller.ok({ team: this.team, ship: this.ship });
    };

    cancel() {
        this.controller.cancel();
    };
}