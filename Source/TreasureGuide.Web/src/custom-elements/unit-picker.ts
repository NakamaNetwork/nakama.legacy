import { bindable, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { UnitQueryService } from '../services/query/unit-query-service';

@autoinject
@customElement('unit-picker')
export class UnitPicker {
    private element: Element;
    private unitQueryService: UnitQueryService;
    @bindable unitId = 0;

    unit = {};
    units: any[];

    constructor(unitQueryService: UnitQueryService, element: Element) {
        this.unitQueryService = unitQueryService;
        this.element = element;
        unitQueryService.stub().then(result => {
            this.units = result;
        });
    }

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