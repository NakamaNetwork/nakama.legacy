import { autoinject } from 'aurelia-dependency-injection';
import { TeamQueryService } from '../../services/query/team-query-service';

@autoinject
export class TeamIndexPage {
    title = 'Teams';
    teams = [];

    constructor(teamQueryService: TeamQueryService) {
        teamQueryService.stub().then(results => {
            this.teams = results;
        });
    }
}