import { autoinject, bindable, customElement } from 'aurelia-framework';
import { UnitSearchModel } from "../services/query/unit-query-service";

@autoinject
@customElement('unit-search')
export class UnitSearch {
    @bindable
    model: UnitSearchModel;

    @bindable
    boxLocked: boolean;

    bind() {
        if (this.model) {
            if (this.boxLocked) {
                this.model.lockedFields.push('boxId');
                this.model.lockedFields.push('blacklist');
            }
        }
    }
}