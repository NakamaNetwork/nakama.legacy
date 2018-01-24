import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { } from 'aurelia-framework';
import { ITeamDetailModel, ITeamUnitDetailModel } from '../models/imported';

@customElement('team-inline')
@autoinject
export class TeamDisplay {
    @bindable team: ITeamDetailModel;

    @computedFrom('team', 'teamUnits', 'teamGenericSlots')
    get slots() {
        var slots = [];
        for (var i = 0; i < 6; i++) {
            var slot = this.team.teamUnits.find(x => x.position === i && !x.sub);
            slots.push(slot);
        }
        return slots;
    }
}