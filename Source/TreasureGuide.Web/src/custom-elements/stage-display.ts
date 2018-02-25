import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-framework';
import { StageQueryService } from '../services/query/stage-query-service';
import { DialogService } from 'aurelia-dialog';
import { StagePicker } from './dialogs/stage-picker';
import { IStageStubModel } from '../models/imported';

@autoinject
@customElement('stage-display')
export class StageDisplay {
    private element: Element;
    private stageQueryService: StageQueryService;
    private dialogService: DialogService;

    @bindable stageId = 0;
    @bindable editable = false;
    @bindable invasion = false;
    stage: IStageStubModel;

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

    @computedFrom('stage', 'stage.unitId')
    get unitId() {
        return this.stage ? this.stage.unitId : null;
    }

    @computedFrom('stageId', 'unitId')
    get iconClass() {
        return (this.unitId) ? '' : ('fa fa-fw fa-' + (this.stageId ? 'map' : 'map-o'));
    }

    @computedFrom('invasion')
    get invasionClass() {
        return this.invasion ? 'invasion' : '';
    }

    @computedFrom('stage', 'stage.name', 'editable', 'invasion')
    get label() {
        return ((this.stage && this.stage.name) ? this.stage.name : ((this.editable ? 'Select' : 'Any') + ' ' + (this.invasion ? 'Invasion' : 'Stage')));
    }

    clicked() {
        if (this.editable) {
            this.dialogService.open({ viewModel: StagePicker, model: { stageId: this.stageId, invasion: this.invasion }, lock: true }).whenClosed(result => {
                if (!result.wasCancelled) {
                    this.stageId = result.output;
                }
            });
        }
    }
}