import { BindingEngine } from 'aurelia-binding';
import { autoinject } from 'aurelia-framework';
import { StageQueryService, StageSearchModel } from '../../services/query/stage-query-service';
import { Router } from 'aurelia-router';
import {SearchModel} from '../../models/search-model';

@autoinject
export class StageIndexPage {
    bindingEngine: BindingEngine;
    stageQueryService: StageQueryService;
    router: Router;

    title = 'Stages';
    stages = [];

    searchModel: StageSearchModel;
    loading;

    constructor(stageQueryService: StageQueryService, bindingEngine: BindingEngine, router: Router) {
        this.stageQueryService = stageQueryService;
        this.bindingEngine = bindingEngine;
        this.router = router;

    }

    activate(params) {
        this.searchModel = <StageSearchModel>new StageSearchModel().getCached();
        if (params) {
            this.searchModel = this.searchModel.assign(params);
        }
        this.bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    search(payload) {
        if (this.stageQueryService) {
            this.loading = true;
            this.router.navigateToRoute('stages', payload, { replace: false, trigger: false });
            this.stageQueryService.search(payload).then(x => {
                this.stages = x.results;
                this.searchModel.totalResults = x.totalResults;
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }
}