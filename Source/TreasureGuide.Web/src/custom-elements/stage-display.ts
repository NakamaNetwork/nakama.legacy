import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { StageQueryService } from '../services/query/stage-query-service';
import { DialogService } from 'aurelia-dialog';
import { StagePicker } from './stage-picker';

@autoinject
@customElement('stage-display')
export class StageDisplay {
    private element: Element;
    private stageQueryService: StageQueryService;
    private dialogService: DialogService;

    @bindable stageId = 0;
    @bindable editable = false;
    stage;

    constructor(stageQueryService: StageQueryService, dialogService: DialogService, element: Element) {
        this.stageQueryService = stageQueryService;
        this.element = element;
        this.dialogService = dialogService;
    }

    stageIdChanged(newValue: number, oldValue: number) {
        return this.stageQueryService.stub(newValue).then(result => {
            this.stage = result;
        }).catch(error => {
            console.error(error);
        });
    };
    
    clicked() {
        if (this.editable) {
            this.dialogService.open({ viewModel: StagePicker, model: { stageId: this.stageId }, lock: true }).whenClosed(result => {
                if (!result.wasCancelled) {
                    this.stageId = result.output;
                }
            });
        }
    }
}