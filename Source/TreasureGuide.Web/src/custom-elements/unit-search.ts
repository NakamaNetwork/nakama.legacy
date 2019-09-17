import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { UnitSearchModel } from "../services/query/unit-query-service";
import { BoxService } from '../services/box-service';

@autoinject
@customElement('unit-search')
export class UnitSearch {
    private boxService: BoxService;

    @bindable
    model: UnitSearchModel;

    @bindable
    boxLocked: boolean;

    @bindable
    inputBox: HTMLInputElement;

    constructor(boxService: BoxService) {
        this.boxService = boxService;
    }

    bind() {
        if (this.model) {
            if (this.boxLocked) {
                this.model.lockedFields.push('myBox');
                this.model.lockedFields.push('limitTo');
            }
        }
    }

    attached() {
        if (this.inputBox) {
            this.inputBox.setSelectionRange(0, this.inputBox.value.length);
        }
    }

    @computedFrom('boxService.currentBox')
    get myBoxDisabled() {
        return this.boxService.currentBox ? '' : 'disabled';
    }

    @computedFrom('boxService.currentBox')
    get myBoxTitle() {
        return this.boxService.currentBox ? 'Filter results based on your current box.' : 'You must open a box first.';
    }
}