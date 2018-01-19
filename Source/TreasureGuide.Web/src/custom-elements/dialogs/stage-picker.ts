import { autoinject, bindable } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { BindingEngine } from 'aurelia-binding';
import { StageQueryService, StageSearchModel } from '../../services/query/stage-query-service';

@autoinject
export class StagePicker {
    private controller: DialogController;
    private stageQueryService: StageQueryService;
    @bindable stageId = 0;

    stage;
    stages: any[];
    
    searchModel = new StageSearchModel().getCached();
    loading;

    constructor(stageQueryService: StageQueryService, controller: DialogController, bindingEngine: BindingEngine) {
        this.controller = controller;
        this.controller.settings.centerHorizontalOnly = true;
        this.stageQueryService = stageQueryService;

        bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    activate(viewModel) {
        this.stageId = viewModel.stageId;
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