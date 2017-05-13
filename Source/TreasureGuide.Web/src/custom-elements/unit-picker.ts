import { bindable } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { UnitQueryService, UnitSearchModel } from '../services/query/unit-query-service';
import { DialogController } from 'aurelia-dialog';
import { BindingEngine } from 'aurelia-binding';

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

    constructor(unitQueryService: UnitQueryService, controller: DialogController, bindingEngine: BindingEngine) {
        this.controller = controller;
        this.controller.settings.centerHorizontalOnly = true;
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

    search(payload) {
        if (this.unitQueryService) {
            this.unitQueryService.search(payload).then(x => {
                this.units = x.results;
                this.resultCount = x.totalResults;
                this.pages = Math.ceil(x.totalResults / payload.pageSize);
            });
        }
    }

    submit() {
        this.controller.ok(this.unit.id);
    };

    cancel() {
        this.controller.cancel();
    };

    clicked(unit) {
        this.unit = unit;
        this.submit();
    }

    getIcon(id: number) {
        if (id) {
            return this.unitQueryService.getIcon(id);
        }
        return null;
    }

    getPortrait(id: number) {
        if (id) {
            return this.unitQueryService.getPortrait(id);
        }
        return null;
    }
}