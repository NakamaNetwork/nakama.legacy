import { bindable, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { StageQueryService } from '../services/query/stage-query-service';

@autoinject
@customElement('stage-picker')
export class StagePicker {
    private element: Element;
    private stageQueryService: StageQueryService;
    @bindable unitId = 0;

    stage = {};
    stages: any[];

    constructor(stageQueryService: StageQueryService, element: Element) {
        this.stageQueryService = stageQueryService;
        this.element = element;
        stageQueryService.stub().then(result => {
            this.stages = result;
        });
    }

    attached() {
        if (this.unitId) {
            this.stageQueryService.stub(this.unitId).then(result => {
                this.stage = result;
            });
        }
    }
}