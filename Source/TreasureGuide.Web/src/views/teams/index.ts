import { BindingEngine } from 'aurelia-binding';
import { autoinject } from 'aurelia-framework';
import { TeamQueryService, TeamSearchModel } from '../../services/query/team-query-service';
import { Router } from 'aurelia-router';

@autoinject
export class TeamIndexPage {
    bindingEngine: BindingEngine;
    teamQueryService: TeamQueryService;
    router: Router;

    title = 'Teams';
    teams = [];

    searchModel: TeamSearchModel;
    loading;

    constructor(teamQueryService: TeamQueryService, bindingEngine: BindingEngine, router: Router) {
        this.teamQueryService = teamQueryService;
        this.bindingEngine = bindingEngine;
        this.router = router;
    }

    activate(params) {
        this.searchModel = new TeamSearchModel();
        if (!this.searchModel.assign(params)) {
            this.searchModel = <TeamSearchModel>this.searchModel.getCached();
        }
        this.bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    search(payload) {
        if (this.teamQueryService) {
            this.loading = true;
            this.router.navigateToRoute('teams', payload, { replace: false, trigger: false });
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