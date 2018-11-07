import { autoinject, bindable } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { BindingEngine } from 'aurelia-binding';
import { StageQueryService, StageSearchModel } from '../../services/query/stage-query-service';
import {StageType} from '../../models/imported';

@autoinject
export class StagePicker {
    private controller: DialogController;
    private stageQueryService: StageQueryService;
    private bindingEngine: BindingEngine;

    @bindable stageId = 0;

    stage;
    stages: any[];
    invasion: boolean;

    searchModel: StageSearchModel;
    loading;

    constructor(stageQueryService: StageQueryService, controller: DialogController, bindingEngine: BindingEngine) {
        this.controller = controller;
        this.controller.settings.centerHorizontalOnly = true;
        this.stageQueryService = stageQueryService;
        this.searchModel = new StageSearchModel().getDefault();
        this.searchModel = <StageSearchModel>this.searchModel.getCached();
        this.bindingEngine = bindingEngine;
    }

    activate(viewModel) {
        this.stageId = viewModel.stageId;
        this.invasion = viewModel.invasion;

        if (this.invasion) {
            this.searchModel.term = 'Invasion';
            this.searchModel.type = StageType.Special;
        }
        this.bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    search(payload) {
        if (this.stageQueryService) {
            this.loading = true;
            this.stageQueryService.search(payload).then(x => {
                this.stages = x.results;
                this.searchModel.totalResults = x.totalResults;
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }

    submit() {
        this.controller.ok(this.stageId);
    };

    cancel() {
        this.controller.cancel();
    };

    clicked(stageId) {
        this.stageId = stageId;
        this.submit();
    }
}