import { BindingEngine } from 'aurelia-binding';
import { autoinject } from 'aurelia-dependency-injection';
import { TeamQueryService, TeamSearchModel } from '../../services/query/team-query-service';

@autoinject
export class TeamIndexPage {
    teamQueryService: TeamQueryService;
    title = 'Teams';
    teams = [];
    pages = 0;

    searchModel = new TeamSearchModel();

    constructor(teamQueryService: TeamQueryService, bindingEngine: BindingEngine) {
        this.teamQueryService = teamQueryService;
        bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    onPageChanged(e) {
        this.searchModel.page = e.detail;
    }

    search(payload) {
        if (this.teamQueryService) {
            this.teamQueryService.search(this.searchModel).then(x => {
                this.teams = x.results;
                this.pages = x.totalPages;
            });
        }
    }
}