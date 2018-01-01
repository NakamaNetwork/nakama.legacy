import { BindingEngine } from 'aurelia-binding';
import { autoinject } from 'aurelia-framework';
import { TeamQueryService, TeamSearchModel } from '../../services/query/team-query-service';

@autoinject
export class TeamIndexPage {
    bindingEngine: BindingEngine;
    teamQueryService: TeamQueryService;
    title = 'Teams';
    teams = [];

    resultCount = 0;
    pages = 0;

    searchModel = new TeamSearchModel();
    loading;

    constructor(teamQueryService: TeamQueryService, bindingEngine: BindingEngine) {
        this.teamQueryService = teamQueryService;
        this.bindingEngine = bindingEngine;
        bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    search(payload) {
        if (this.teamQueryService) {
            this.loading = true;
            this.teamQueryService.search(payload).then(x => {
                this.teams = x.results;
                this.resultCount = x.totalResults;
                this.pages = Math.ceil(x.totalResults / payload.pageSize);
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }
}