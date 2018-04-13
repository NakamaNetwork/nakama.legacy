import { autoinject, computedFrom, BindingEngine } from 'aurelia-framework';
import { GCRQueryService } from '../../services/query/gcr-query-service';
import { IGCRResultModel } from '../../models/imported';

@autoinject
export class SupportIndexPage {
    private gcrQueryService: GCRQueryService;
    private bindingEngine: BindingEngine;

    private loading: boolean;
    private gcr: IGCRResultModel;

    constructor(gcrQueryService: GCRQueryService, bindingEngine: BindingEngine) {
        this.gcrQueryService = gcrQueryService;
        this.bindingEngine = bindingEngine;
    }

    activate() {
        this.loading = true;
        this.gcrQueryService.gcrTable().then(x => {
            this.gcr = x;
            this.loading = false;
        }).catch((e) => {
            this.loading = false;
        });
    }
}