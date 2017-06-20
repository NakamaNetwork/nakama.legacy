import { bindable } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { StageQueryService, StageSearchModel } from '../services/query/stage-query-service';
import { DialogController } from 'aurelia-dialog';
import { BindingEngine } from 'aurelia-binding';

@autoinject
export class StagePicker {
    private controller: DialogController;
    private stageQueryService: StageQueryService;
    @bindable stageId = 0;

    stage;
    stages: any[];

    resultCount = 0;
    pages = 0;

    searchModel = new StageSearchModel();

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

    onPageChanged(e) {
        this.searchModel.page = e.detail;
    }

    search(payload) {
        if (this.stageQueryService) {
            this.stageQueryService.search(payload).then(x => {
                this.stages = x.results;
                this.resultCount = x.totalResults;
                this.pages = Math.ceil(x.totalResults / payload.pageSize);
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