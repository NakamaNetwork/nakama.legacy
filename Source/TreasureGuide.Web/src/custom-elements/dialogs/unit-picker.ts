import { autoinject, bindable } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { BindingEngine } from 'aurelia-binding';
import { UnitQueryService, UnitSearchModel } from '../../services/query/unit-query-service';

@autoinject
export class UnitPicker {
    private controller: DialogController;
    private unitQueryService: UnitQueryService;
    @bindable unitId = 0;

    unit;
    units: any[];

    resultCount = 0;
    pages = 0;

    searchModel = new UnitSearchModel();
    loading;

    constructor(unitQueryService: UnitQueryService, controller: DialogController, bindingEngine: BindingEngine) {
        this.controller = controller;
        this.unitQueryService = unitQueryService;

        bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    activate(viewModel) {
        this.unitId = viewModel.unitId;
    }

    onPageChanged(e) {
        this.searchModel.page = e.detail;
    }

    search(payload: UnitSearchModel) {
        if (this.unitQueryService) {
            this.loading = true;
            this.unitQueryService.search(payload).then(x => {
                this.units = x.results;
                this.resultCount = x.totalResults;
                this.pages = Math.ceil(x.totalResults / payload.pageSize);
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }

    submit() {
        this.controller.ok(this.unitId);
    };

    cancel() {
        this.controller.cancel();
    };

    clicked(unitId) {
        this.unitId = unitId;
        this.submit();
    }

    getIcon(id: number) {
        return UnitQueryService.getIcon(id);
    }
}