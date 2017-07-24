import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { DialogService } from 'aurelia-dialog';
import { TeamImportView } from './team-import';

@customElement('team-display')
@autoinject
export class TeamDisplay {
    private element: Element;
    private dialogService: DialogService;

    @bindable team: any[] = [];
    @bindable ship: number = 0;
    @bindable editable = false;

    constructor(element: Element, dialogService: DialogService) {
        this.element = element;
        this.dialogService = dialogService;
    }

    @computedFrom('team')
    get teamSlots() {
        if (!this.team) {
            this.team = [];
        }
        var mainUnits = this.team.filter(x => {
            return !x.sub;
        });
        var slots = [];
        for (var i = 0; i < 6; i++) {
            var unit = mainUnits.find(x => {
                return x.position === i;
            });
            if (!unit) {
                unit = { unitId: 0, position: i };
                this.team.push(unit);
            }
            slots.push(unit);
        }
        return slots;
    }

    openImport() {
        if (this.editable) {
            this.dialogService.open({ viewModel: TeamImportView, lock: true }).whenClosed(result => {
                if (!result.wasCancelled) {
                    this.team = result.output.team;
                    this.ship = result.output.ship;
                }
            });
        }
    };

    private getSlot(id: number) {
        return this.team.filter(unit => {
            return unit.position === id;
        });
    };
}