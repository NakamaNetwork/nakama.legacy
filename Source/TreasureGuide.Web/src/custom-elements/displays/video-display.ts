import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { ITeamVideoModel } from '../../models/imported';
import { AccountService } from '../../services/account-service';
import { TeamQueryService } from '../../services/query/team-query-service';
import { RoleConstants } from '../../models/imported';

@autoinject
@customElement('video-display')
export class VideoDisplay {
    accountService: AccountService;
    teamQueryService: TeamQueryService;

    @bindable video: ITeamVideoModel;

    constructor(accountService: AccountService, teamQueryService: TeamQueryService) {
        this.accountService = accountService;
        this.teamQueryService = teamQueryService;
    }

    @computedFrom('video', 'video.userName', 'accountService.userProfile')
    get canDelete(): boolean {
        return this.accountService.isInRoles([RoleConstants.Administrator, RoleConstants.Moderator]) ||
            this.accountService.userProfile.userName === this.video.userName;
    }

    delete() {
        if (this.canDelete) {
            var clone = Object.assign(<ITeamVideoModel>{}, this.video);
            clone.deleted = true;
            this.teamQueryService.video(clone);
        }
    }
}