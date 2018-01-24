import { autoinject, bindable } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { CalcParser } from '../../tools/calc-parser';
import { TeamEditorModel } from '../../services/query/team-query-service';

@autoinject
export class TeamImportView {
    private controller: DialogController;

    @bindable input = '';

    team = new TeamEditorModel();

    constructor(controller: DialogController) {
        this.controller = controller;
    }

    inputChanged(newValue: string, oldValue: string) {
        var teamIds = CalcParser.parse(this.input);
        var teamSet = CalcParser.convert(teamIds.units);
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