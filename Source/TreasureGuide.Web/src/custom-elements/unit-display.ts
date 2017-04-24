import { bindable, observable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { UnitQueryService } from '../services/query/unit-query-service';

@autoinject
@customElement('unit-display')
export class UnitDisplay {
    private element: Element;
    private unitQueryService: UnitQueryService;
    @bindable right = false;
    @bindable @observable unitId = 0;
    @bindable editable = false;

    constructor(unitQueryService: UnitQueryService, element: Element) {
        this.unitQueryService = unitQueryService;
        this.element = element;
    }

    @computedFrom('unitId')
    get unit() {
        return this.unitQueryService.stub(this.unitId).then(result => {
            return result;
        }).catch(error => {
            console.error(error);
            return null;
        });
    };
    
    @computedFrom('unitId')
    get imageUrl() {
        return this.unitQueryService.getPortrait(this.unitId);
    }

    unitClicked() {
        if (this.editable) {
            console.log("Clicked editable unit");
        } else {
            console.log("Clicked uneditable unit.");
        }
    }
}