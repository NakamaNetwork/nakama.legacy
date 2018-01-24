import { autoinject, bindable } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { CalcParser } from '../../tools/calc-parser';
import { TeamEditorModel } from '../../services/query/team-query-service';

@autoinject
export class TeamImportView {
    private controller: DialogController;
    private calcParser: CalcParser;

    @bindable input = '';

    team = new TeamEditorModel();

    constructor(calcParser: CalcParser, controller: DialogController) {
        this.controller = controller;
        this.calcParser = calcParser;
    }

    inputChanged(newValue: string, oldValue: string) {
        var teamIds = this.calcParser.parse(this.input);
        var teamSet = this.calcParser.convert(teamIds.units);
        this.team.teamUnits = teamSet;
        this.team.shipId = teamIds.ship;
    }

    submit() {
        this.controller.ok({ team: this.team.teamUnits, ship: this.team.shipId });
    };

    cancel() {
        this.controller.cancel();
    };
}