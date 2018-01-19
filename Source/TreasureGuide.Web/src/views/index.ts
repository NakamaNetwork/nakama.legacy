import { autoinject } from 'aurelia-framework';
import { TeamQueryService } from '../services/query/team-query-service';
import { ITeamStubModel } from '../models/imported';
import { Router } from 'aurelia-router';

@autoinject
export class HomePage {
    private teamQueryService: TeamQueryService;
    private router: Router;

    private newLoading: boolean = true;
    private trendingLoading: boolean = true;
    private newTeams: ITeamStubModel[] = [];
    private trendingTeams: ITeamStubModel[] = [];

    constructor(teamQueryService: TeamQueryService, router: Router) {
        this.teamQueryService = teamQueryService;
        this.router = router;
    }

    activate(params) {
        if (!localStorage['visited']) {
            localStorage['visited'] = true;
            this.router.navigateToRoute('about');
        } else {
            this.teamQueryService.latest().then(x => {
                this.newTeams = x;
                this.newLoading = false;
            }).catch(x => {
                this.newLoading = false;
            });
            this.teamQueryService.trending().then(x => {
                this.trendingTeams = x;
                this.trendingLoading = false;
            }).catch(x => {
                this.trendingLoading = false;
            });
        }
    }
}