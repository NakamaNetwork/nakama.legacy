import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
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
        return this.team.filter(x => {
            return !x.sub;
        }).sort((a, b) => {
            return a.position - b.position;
        }).map(x => {
            if (x.unitId !== undefined) {
                return x;
            } else {
                return { unitId: x };
            }
        });
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