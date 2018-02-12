import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { TeamQueryService } from '../../services/query/team-query-service';
import { AlertService } from '../../services/alert-service';
import { DialogService } from 'aurelia-dialog';
import { ReportDialog } from '../dialogs/report-dialog';

@autoinject
@customElement('score-display')
export class ScoreDisplay {
    private teamQueryService: TeamQueryService;
    private alertService: AlertService;
    private dialogService: DialogService;

    @bindable score: number;
    @bindable teamId: number;
    @bindable myVote: number;
    @bindable myBookmark: boolean;
    @bindable votable: boolean;

    constructor(teamQueryService: TeamQueryService, alertService: AlertService, dialogService: DialogService) {
        this.teamQueryService = teamQueryService;
        this.alertService = alertService;
        this.dialogService = dialogService;
    }

    @computedFrom('score', 'myVote')
    get scoreClass() {
        var className = 'score-text';
        if (this.score > 0) {
            className += ' positive';
        } else if (this.score < 0) {
            className += ' negative';
        }
        return className;
    }

    @computedFrom('myVote')
    get thumbsUpIcon() {
        if (this.myVote > 0) {
            return 'fa-thumbs-up';
        }
        return 'fa-thumbs-o-up';
    }

    @computedFrom('myVote')
    get thumbsDownIcon() {
        if (this.myVote < 0) {
            return 'fa-thumbs-down';
        }
        return 'fa-thumbs-o-down';
    }

    @computedFrom('myBookmark')
    get bookmarkIcon() {
        if (this.myBookmark) {
            return 'fa-heart _dangerText';
        }
        return 'fa-heart-o';
    }

    thumbsUp() {
        if (this.myVote > 0) {
            this.vote(null);
        } else {
            this.vote(true);
        }
    }

    thumbsDown() {
        if (this.myVote < 0) {
            this.vote(null);
        } else {
            this.vote(false);
        }
    }

    vote(score?: boolean) {
        this.teamQueryService.vote({ teamId: this.teamId, up: score }).then(result => {
            this.alertService.success('Your vote has been recorded!');
            this.score = result;
            this.myVote = score != null ? score ? 1 : -1 : 0;
        }).catch(response => this.alertService.reportError(response));
    }

    bookmark() {
        this.teamQueryService.bookmark(this.teamId).then(result => {
            this.myBookmark = result;
            var message = result ? 'This team has been bookmarked.' : 'Removed team bookmark.';
            this.alertService.success(message);
        }).catch(response => this.alertService.reportError(response));
    }

    report() {
        this.dialogService.open({ viewModel: ReportDialog, lock: true }).whenClosed(result => {
            if (!result.wasCancelled) {
                this.teamQueryService.report({ teamId: this.teamId, reason: <string>result.output }).then(result => {
                    this.alertService.success('Thank you! Your report has been submitted.');
                }).catch(response => this.alertService.reportError(response));
            }
        });
    }
}