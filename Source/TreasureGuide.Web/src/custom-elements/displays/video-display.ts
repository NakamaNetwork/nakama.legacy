import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { ITeamVideoModel } from '../../models/imported';

@customElement('video-display')
export class VideoDisplay {
    @bindable video: ITeamVideoModel;
    @bindable submitterId: string;

    @computedFrom('submitterId', 'video', 'video.userId')
    get isOwner() {
        return this.submitterId === this.video.userId;
    }
}