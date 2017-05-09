import { bindable } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { UnitQueryService } from '../services/query/unit-query-service';
import { DialogController } from 'aurelia-dialog';

@autoinject
export class UnitPicker {
    private controller: DialogController;
    private unitQueryService: UnitQueryService;
    @bindable unitId = 0;

    unit;
    units: any[];

    constructor(unitQueryService: UnitQueryService, controller: DialogController) {
        this.controller = controller;
        this.controller.settings.centerHorizontalOnly = true;
        this.unitQueryService = unitQueryService;
        unitQueryService.stub().then(result => {
            this.units = result;
        });
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