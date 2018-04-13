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
    get teams() {
        return this.gcr.teams.filter(x => x.leaderId == this.unitId && x.stageId == this.stageId);
    }

    @computedFrom('teams')
    get teamCount() {
        return this.teams.length;
    }

    @computedFrom('teams')
    get fullTeam() {
        return this.teams.find(x => x.f2P && x.global && x.video);
    }

    @computedFrom('teams')
    get f2Pglobal() {
        return this.teams.find(x => x.f2P && x.global);
    }

    @computedFrom('teams')
    get f2PVideo() {
        return this.teams.find(x => x.f2P && x.video);
    }

    @computedFrom('teams')
    get globalVideo() {
        return this.teams.find(x => x.global && x.video);
    }

    @computedFrom('teams')
    get f2P() {
        return this.teams.find(x => x.f2P);
    }

    @computedFrom('teams')
    get global() {
        return this.teams.find(x => x.global);
    }

    @computedFrom('teams')
    get video() {
        return this.teams.find(x => x.video);
    }

    @computedFrom('fullTeam', 'f2PGlobal', 'f2PVideo', 'globalVideo', 'f2P', 'global', 'video')
    get bestTeam() {
        return this.fullTeam || this.f2Pglobal || this.f2PVideo || this.f2P || this.global || this.video;
    }
}