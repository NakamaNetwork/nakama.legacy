import { bindable, observable, computedFrom, customElement } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { UnitQueryService } from '../services/query/unit-query-service';

@customElement('unit-display')
@autoinject
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
        var id = ("0000" + this.unitId).slice(-4);
        return 'https://onepiece-treasurecruise.com/wp-content/uploads/f' + id + '.png';
    }
}