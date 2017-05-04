import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';

@customElement('team-display')
@autoinject
export class TeamDisplay {
    private element: Element;

    importDialog;
    @bindable team: any[] = [];
    @bindable editable = false;
    @bindable({ changeHandler: 'imported' }) import;

    constructor(element: Element) {
        this.element = element;
    }

    @computedFrom('team')
    get teamSlots() {
        var slots;
        if (this.team) {
            slots = this.team.filter(x => {
                return !x.sub;
            }).sort((a, b) => {
                return a.position - b.position;
            }).map(x => {
                return x.unitId;
            });
        } else {
            slots = new Array(6);
        }
        return slots;
    }

    imported(newValue, oldVAlue) {
        this.team = newValue;
    }

    openImport() {
        if (this.editable) {
            this.importDialog.open();
        }
    };

    private getSlot(id: number) {
        return this.team.filter(unit => {
            return unit.position === id;
        });
    };
}