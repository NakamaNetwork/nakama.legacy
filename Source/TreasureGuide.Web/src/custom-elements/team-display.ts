import { bindable, observable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';

@customElement('team-display')
@autoinject
export class TeamDisplay {
    private element: Element;

    importDialog;
    @bindable @observable team: any[];
    @bindable editable = false;

    constructor(element: Element) {
        this.element = element;
    }

    @computedFrom('team')
    get teamSlots() {
        var slots = this.team.filter(x => {
            return !x.sub;
        }).sort((a, b) => {
            return a.position - b.position;
        }).map(x => {
            return x.position || x;
        });
        return slots;
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