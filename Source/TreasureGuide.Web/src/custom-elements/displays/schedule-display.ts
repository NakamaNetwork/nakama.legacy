import { autoinject, bindable, computedFrom, customElement, BindingEngine } from 'aurelia-framework';
import { StageQueryService } from '../../services/query/stage-query-service';
import { IScheduleModel, IStageStubModel } from '../../models/imported';
import { ArrayHelper } from '../../tools/array-helper';

@autoinject
@customElement('schedule-display')
export class ScheduleDisplay {
    private bindingEngine: BindingEngine;
    private stageQueryService: StageQueryService;

    private scheduleLoading: boolean = true;
    private fullSchedule: IScheduleModel;
    private currentSchedule = [];
    private upcomingSchedule = [];
    private globalSchedule: boolean = true;

    private globalKey: string = 'global_schedule';

    constructor(stageQueryService: StageQueryService, bindingEngine: BindingEngine) {
        this.stageQueryService = stageQueryService;
        this.bindingEngine = bindingEngine;

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