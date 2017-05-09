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

    onPageChanged(e) {
        this.searchModel.page = e.detail;
    }

    search(payload) {
        if (this.unitQueryService) {
            this.unitQueryService.search(this.searchModel).then(x => {
                this.units = x.results;
                this.pages = x.totalPages;
            });
        }
    }

    submit() {
        this.controller.ok(this.unit.id);
    };

    cancel() {
        this.controller.cancel();
    };

    attached() {
        if (this.unitId) {
            this.unitQueryService.stub(this.unitId).then(result => {
                this.unit = result;
            });
        }
    }

    unitClicked(unit) {
        this.unit = unit;
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