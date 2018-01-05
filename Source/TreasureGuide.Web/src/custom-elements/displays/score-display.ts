import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { TeamQueryService } from '../../services/query/team-query-service';

@customElement('score-display')
export class ScoreDisplay {
    @bindable score: number;
    @bindable teamId: number;
    @bindable myScore: number;
    @bindable votable: boolean;

    @computedFrom('score', 'myScore')
    get scoreClass() {
        var className = 'score-text';
        if (this.score > 0) {
            className += ' positive';
        } else if (this.score < 0) {
            className += ' negative';
        }
        if (this.myScore > 0) {
            className += ' my-vote my-positive';
        } else if (this.myScore < 0) {
            className += ' my-vote my-negative';
        }
        return className;
    }
}