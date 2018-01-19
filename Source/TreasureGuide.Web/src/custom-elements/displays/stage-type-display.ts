import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { StageType } from '../../models/imported';

@customElement('stage-type-display')
export class StageTypeDisplay {
    @bindable type = 0;

    @computedFrom('type')
    get text() {
        return StageType[this.type];
    }
}