import { autoinject, bindable, computedFrom, customElement } from 'aurelia-framework';
import { NumberHelper } from '../../tools/number-helper';
import { IndividualUnitFlags } from '../../models/imported';
import { NameIdPair } from '../../models/name-id-pair';

@autoinject
@customElement('ind-unit-flag-display')
export class IndUnitFlagDisplay {
    @bindable
    value: number;

    private possible: NameIdPair[];

    constructor() {
        this.possible = NumberHelper.getPairs(IndividualUnitFlags, true);
    }

    @computedFrom('value')
    get values() {
        return this.value ? this.possible.filter(x => (this.value & x.id) === x.id) : [];
    }
}