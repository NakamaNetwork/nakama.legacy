import { BindingEngine } from 'aurelia-binding';
import { autoinject } from 'aurelia-framework';
import { TeamQueryService, TeamSearchModel } from '../../services/query/team-query-service';

@autoinject
export class TeamIndexPage {
    bindingEngine: BindingEngine;
    teamQueryService: TeamQueryService;
    title = 'Teams';
    teams = [];

    searchModel = new TeamSearchModel().getCached();
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
                this.searchModel.totalResults = x.totalResults;
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }
}