import { autoinject } from 'aurelia-framework';
import { TeamQueryService } from '../services/query/team-query-service';
import { ITeamStubModel } from '../models/imported';
import { Router } from 'aurelia-router';

@autoinject
export class HomePage {
    private teamQueryService: TeamQueryService;

    private newLoading: boolean = true;
    private newError: boolean = false;
    private newTeams: ITeamStubModel[] = [];

    private trendingLoading: boolean = true;
    private trendingError: boolean = false;
    private trendingTeams: ITeamStubModel[] = [];

    constructor(teamQueryService: TeamQueryService) {
        this.teamQueryService = teamQueryService;
    }

    activate(params) {
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