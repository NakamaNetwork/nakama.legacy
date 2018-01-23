import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { ITeamDetailModel, ITeamUnitEditorModel } from '../models/imported';

@customElement('team-display')
@autoinject
export class TeamDisplay {
    @bindable team: ITeamDetailModel;
    @bindable editable = false;

    @computedFrom('team')
    get slots() {
        var slots = [];
        for (var i = 0; i < 6; i++) {
            var units = this.team.teamUnits.filter(x => x.position === i);
            var generic = this.team.teamGenericSlots.filter(x => x.position === i);

            var slot = <any[]>units.concat(<any[]>generic).sort((a, b) => {
                return (a.sub ? 1 : -1) - (b.sub ? 1 : -1);
            });
            slots.push(slot);
        }
        return slots;
    }
}