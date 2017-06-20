import { BindingEngine } from 'aurelia-binding';
import { autoinject } from 'aurelia-dependency-injection';
import { TeamQueryService, TeamSearchModel } from '../../services/query/team-query-service';

@autoinject
export class TeamIndexPage {
    teamQueryService: TeamQueryService;
    title = 'Teams';
    teams = [];

    resultCount = 0;
    pages = 0;

    searchModel = new TeamSearchModel();
    searchTimer;

    constructor(teamQueryService: TeamQueryService, bindingEngine: BindingEngine) {
        this.teamQueryService = teamQueryService;
        bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            if (this.searchTimer) {
                clearTimeout(this.searchTimer);
            }
            this.searchTimer = setTimeout(() => {
                this.search(n);
            }, 500);
        });
        this.search(this.searchModel.payload);
    }

    onPageChanged(e) {
        this.searchModel.page = e.detail;
    }

    search(payload) {
        if (this.teamQueryService) {
            this.teamQueryService.search(payload).then(x => {
                this.teams = x.results;
                this.resultCount = x.totalResults;
                this.pages = Math.ceil(x.totalResults / payload.pageSize);
            });
        }
    }
}