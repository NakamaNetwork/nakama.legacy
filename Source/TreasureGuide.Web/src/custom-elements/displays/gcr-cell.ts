import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { IGCRResultModel } from '../../models/imported';

@customElement('gcr-cell')
export class GCRCell {
    @bindable
    public stageId: number;

    @bindable
    public unitId: number;

    @bindable
    public gcr: IGCRResultModel;

    @computedFrom('gcr', 'stageId', 'unintId')
    get team() {
        return this.gcr.teams.find(x => x.leaderId == this.unitId && x.stageId == this.stageId);
    }

    @computedFrom('team')
    get complete() {
        return this.team && this.team.f2P && this.team.global && this.team.video;
    }

    @computedFrom('team', 'complete')
    get incomplete() {
        return this.team && !this.complete && (this.team.f2P || this.team.global || this.team.video);
    }

    @computedFrom('complete', 'incomplete')
    get empty() {
        return !(this.team || this.complete || this.incomplete);
    }

    @computedFrom('team', 'complete', 'empty')
    get style() {
        if (this.empty) {
            return 'empty';
        } else if (this.complete) {
            return 'complete';
        } else if (this.incomplete) {
            return 'incomplete';
        }
    }
}