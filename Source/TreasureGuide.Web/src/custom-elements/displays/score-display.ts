import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { TeamQueryService } from '../../services/query/team-query-service';
import { AlertService } from '../../services/alert-service';

@autoinject
@customElement('score-display')
export class ScoreDisplay {
    private teamQueryService: TeamQueryService;
    private alertService: AlertService;

    @bindable score: number;
    @bindable teamId: number;
    @bindable myVote: number;
    @bindable votable: boolean;

    constructor(teamQueryService: TeamQueryService, alertService: AlertService) {
        this.teamQueryService = teamQueryService;
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
        this.teamQueryService.vote({ teamId: this.teamId, up: score }).then(result => {
            this.alertService.success('Your vote has been recorded!');
            this.score = result;
            this.myVote = score != null ? score ? 1 : -1 : 0;
        }).catch(err => {
            err.text().then(msg => {
                this.alertService.danger(msg);
            });
        });
    }
}