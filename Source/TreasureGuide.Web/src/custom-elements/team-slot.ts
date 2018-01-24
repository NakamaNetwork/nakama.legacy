import { bindable, computedFrom, customElement } from "aurelia-framework";
import { ITeamEditorModel } from '../models/imported';
import { ITeamUnitEditorModel } from '../models/imported';

@customElement('team-slot')
export class TeamSlot {
    @bindable team: ITeamEditorModel;
    @bindable editable: boolean;
    @bindable index: number;
    private lastUpdated: Date;

    @computedFrom('team', 'team.teamUnits', 'index', 'lastUpdated')
    get units() {
        var units = this.team.teamUnits.filter(x => x.position === this.index && x.unitId);
        console.log(units);
        return units;
    }

    @computedFrom('units')
    get main() {
        return this.units.find(x => !x.sub);
    }

    @computedFrom('units')
    get subs() {
        return this.units.filter(x => x.sub);
    }

    @computedFrom('main', 'edtiable')
    get canAddMain() {
        return this.editable && !this.main;
    }

    @computedFrom('main', 'subs', 'editable')
    get canAddSubs() {
        return this.editable && this.main && this.subs.length < 4;
    }

    addUnit(event: CustomEvent, sub: boolean) {
        var id = event.detail.unitId;
        if (id) {
            event.detail.viewModel.unitId = 0;
            this.team.teamUnits.push(<ITeamUnitEditorModel>{
                unitId: id,
                position: this.index,
                sub: sub
            });
            this.lastUpdated = new Date();
        }
    }

    checkRemoved(event: CustomEvent, sub: boolean) {
        var id = event.detail.unitId;
        if (!id) {
            var oldId = event.detail.oldUnitId;
            if (oldId) {
                var old = this.team.teamUnits.findIndex(x => x.position === this.index && x.unitId === oldId);
                if (old > -1) {
                    this.team.teamUnits.slice(old, 1);
                    if (!sub) {
                        setTimeout(() => {
                            var next = this.team.teamUnits.find(x => x.position === this.index &&
                                x.unitId &&
                                x.sub);
                            if (next) {
                                next.sub = false;
                            }
                            this.lastUpdated = new Date();
                        }, 0);
                    }
                }
            }
        }
        this.lastUpdated = new Date();
    }
}