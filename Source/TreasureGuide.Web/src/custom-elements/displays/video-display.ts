import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { ITeamVideoModel } from '../../models/imported';
import { AccountService } from '../../services/account-service';
import { TeamQueryService } from '../../services/query/team-query-service';
import { RoleConstants } from '../../models/imported';
import { DialogService } from 'aurelia-dialog';
import { AlertService } from '../../services/alert-service';
import { AlertDialog } from '../dialogs/alert-dialog';

@autoinject
@customElement('video-display')
export class VideoDisplay {
    accountService: AccountService;
    teamQueryService: TeamQueryService;
    dialogService: DialogService;
    alertService: AlertService;

    @bindable video: ITeamVideoModel;

    constructor(
        accountService: AccountService,
        teamQueryService: TeamQueryService,
        dialogService: DialogService,
        alertService: AlertService
    ) {
        this.accountService = accountService;
        this.teamQueryService = teamQueryService;
        this.dialogService = dialogService;
        this.alertService = alertService;
    }

    @computedFrom('video', 'video.deleted')
    get videoClass() {
        return 'video-display ' + (this.video.deleted ? 'deleted' : '');
    }

    @computedFrom('video', 'video.userName', 'video.deleted', 'accountService.userProfile')
    get canDelete(): boolean {
        return !this.video.deleted && (this.accountService.isInRoles([RoleConstants.Administrator, RoleConstants.Moderator]) ||
            (this.accountService.userProfile.userName === this.video.userName));
    }

    delete() {
        if (this.canDelete) {
            this.dialogService.open({
                viewModel: AlertDialog,
                model: { message: 'Are you sure you want to remove this video?', cancelable: true },
                lock: true
            }).whenClosed(result => {
                if (!result.wasCancelled) {
                    var clone = Object.assign({}, this.video);
                    clone.deleted = true;
                    this.teamQueryService.video(clone).then(x => {
                        this.video.deleted = true;
                        this.alertService.success('Successfully deleted video.');
                    }).catch(x => {
                        this.alertService.danger('Could not delete video. Please try again.');
                    });
                }
            });
        }
    }
}