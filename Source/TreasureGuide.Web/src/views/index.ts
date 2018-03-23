import { autoinject, BindingEngine } from 'aurelia-framework';
import { TeamQueryService } from '../services/query/team-query-service';
import { ITeamStubModel, IScheduleModel, IStageStubModel } from '../models/imported';
import { Router } from 'aurelia-router';
import { StageQueryService } from '../services/query/stage-query-service';
import { ArrayHelper } from '../tools/array-helper';

@autoinject
export class HomePage {
    private teamQueryService: TeamQueryService;
    private stageQueryService: StageQueryService;
    private router: Router;
    private bindingEngine: BindingEngine;

    private scheduleLoading: boolean = true;
    private fullSchedule: IScheduleModel;
    private currentSchedule = [];
    private upcomingSchedule = [];
    private globalSchedule: boolean = true;

    private newLoading: boolean = true;
    private newTeams: ITeamStubModel[] = [];

    private trendingLoading: boolean = true;
    private trendingTeams: ITeamStubModel[] = [];

    private globalKey: string = 'global_schedule';

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


            var global = localStorage.getItem(this.globalKey);
            if (global) {
                this.globalSchedule = JSON.parse(global);
            }
            this.refreshSchedule();
            this.bindingEngine.propertyObserver(this, 'globalSchedule').subscribe((n, o) => {
                localStorage.setItem(this.globalKey, JSON.stringify(n));
                this.refreshSchedule();
            });
        }
    }

    refreshSchedule() {
        this.scheduleLoading = true;
        this.stageQueryService.schedule().then(x => {
            this.fullSchedule = x;
            this.stageQueryService.get().then(y => {
                this.currentSchedule = this.getEvents(x.live, this.globalSchedule, y);
                this.upcomingSchedule = this.getEvents(x.upcoming, this.globalSchedule, y);
                this.scheduleLoading = false;
            }).catch(x => {
                this.scheduleLoading = false;
            });
        }).catch(x => {
            this.scheduleLoading = false;
        });
    }

    getEvents(eventData, global: boolean, stages: IStageStubModel[]): any[] {
        var source = global ? eventData.global : eventData.japan;
        var mapping = stages = source.map(x => stages.find(y => y.id == x));
        var bins = ArrayHelper.binBy(mapping, 'type');
        return bins;
    }
}