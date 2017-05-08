import { autoinject } from 'aurelia-dependency-injection';
import { TeamQueryService, TeamSearchModel } from '../../services/query/team-query-service';

@autoinject
export class TeamIndexPage {
    teamQueryService: TeamQueryService;
    title = 'Teams';
    teams = [];

    searchModel = new TeamSearchModel();

    constructor(teamQueryService: TeamQueryService) {
        this.teamQueryService = teamQueryService;
        this.search();
    }

    search() {
        this.teamQueryService.search(this.searchModel).then(x => {
            this.teams = x;
        });
    }
}