import { bindable, observable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';

@customElement('team-display')
@autoinject
export class TeamDisplay {
    private element: Element;
    @bindable @observable team: any[];
    @bindable editable = false;

    constructor(element: Element) {
        this.element = element;
    }

    @computedFrom('team')
    get viewModel() {
        var teamSlots = [];
        for (var i = 1; i < 6; i++) {
            teamSlots.push(this.getSlot(i));
        }
        return teamSlots;
    }

    private getSlot(id: number) {
        return this.team.filter(unit => {
            return unit.position === id;
        });
    };
}