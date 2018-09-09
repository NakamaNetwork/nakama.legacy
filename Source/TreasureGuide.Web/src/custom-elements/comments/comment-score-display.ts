import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { AlertService } from '../../services/alert-service';
import { TeamCommentQueryService } from '../../services/query/team-comment-query-service';

@autoinject
@customElement('comment-score-display')
export class CommentScoreDisplay {
    private teamCommentQueryService: TeamCommentQueryService;
    private alertService: AlertService;

    @bindable score: number;
    @bindable teamCommentId: number;
    @bindable myVote: number;
    @bindable votable: boolean;

    constructor(teamCommentQueryService: TeamCommentQueryService, alertService: AlertService) {
        this.teamCommentQueryService = teamCommentQueryService;
        this.alertService = alertService;
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
        this.teamCommentQueryService.vote({ teamCommentId: this.teamCommentId, up: score }).then(result => {
            this.alertService.success('Your vote has been recorded!');
            this.score = result;
            this.myVote = score != null ? score ? 1 : -1 : 0;
        }).catch(response => this.alertService.reportError(response));
    }

    report() {
        this.teamCommentQueryService.report({ teamCommentId: this.teamCommentId }).then(result => {
            this.alertService.success('Thank you! Your report has been submitted.');
        }).catch(response => this.alertService.reportError(response));
    }
}