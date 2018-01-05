import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';

@customElement('team-display')
@autoinject
export class TeamDisplay {
    private element: Element;

    @bindable team: any[] = [];
    @bindable editable = false;

    constructor(element: Element) {
        this.element = element;
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

    private getSlot(id: number) {
        return this.team.filter(unit => {
            return unit.position === id;
        });
    };
}