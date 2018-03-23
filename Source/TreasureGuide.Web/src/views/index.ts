import { autoinject, BindingEngine } from 'aurelia-framework';
import { TeamQueryService } from '../services/query/team-query-service';
import { ITeamStubModel, IScheduleModel } from '../models/imported';
import { Router } from 'aurelia-router';
import { StageQueryService } from '../services/query/stage-query-service';

@autoinject
export class HomePage {
    private teamQueryService: TeamQueryService;
    private stageQueryService: StageQueryService;
    private router: Router;
    private bindingEngine: BindingEngine;

    private scheduleLoading: boolean = true;
    private schedule: IScheduleModel;
    private globalSchedule: boolean = true;

    private newLoading: boolean = true;
    private newTeams: ITeamStubModel[] = [];

    private trendingLoading: boolean = true;
    private trendingTeams: ITeamStubModel[] = [];

    constructor(teamQueryService: TeamQueryService, stageQueryService: StageQueryService, bindingEngine: BindingEngine, router: Router) {
        this.teamQueryService = teamQueryService;
        this.stageQueryService = stageQueryService;
        this.router = router;
        this.bindingEngine = bindingEngine;
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

            this.refreshSchedule();
            this.bindingEngine.propertyObserver(this.globalSchedule, 'payload').subscribe((n, o) => {
                this.refreshSchedule();
            });
        }
    }

    refreshSchedule() {
        this.scheduleLoading = true;
        this.stageQueryService.schedule().then(x => {
            this.schedule = x;
            this.scheduleLoading = false;
        });
    }
}