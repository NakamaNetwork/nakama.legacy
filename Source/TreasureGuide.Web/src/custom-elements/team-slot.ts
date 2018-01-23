import { bindable, customElement } from "aurelia-framework";


@customElement('team-slot')
export class TeamSlot {
    @bindable teamSlot: any;
    @bindable editable: boolean;
}