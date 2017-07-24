import { BindingEngine } from 'aurelia-binding';
import { autoinject } from 'aurelia-framework';
import { TeamQueryService } from '../../services/query/team-query-service';
import { TeamSearchModel } from '../../models/imported';

@autoinject
export class TeamIndexPage {
    private self: TeamIndexPage;

    bindingEngine: BindingEngine;
    teamQueryService: TeamQueryService;
    title = 'Teams';
    teams = [];

    resultCount = 0;
    pages = 0;

    searchModel = new TeamSearchModel();

    constructor(teamQueryService: TeamQueryService, bindingEngine: BindingEngine) {
        this.teamQueryService = teamQueryService;
        this.bindingEngine = bindingEngine;
        this.searchModel.onChanged = this.search;
        this.self = this;
    }

    bind() {
        this.search(this.searchModel);
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