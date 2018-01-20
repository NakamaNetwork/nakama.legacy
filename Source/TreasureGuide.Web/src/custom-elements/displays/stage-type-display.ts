import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { StageType } from '../../models/imported';
import { StringHelper } from '../../tools/string-helper';

@customElement('stage-type-display')
export class StageTypeDisplay {
    @bindable type = 0;

    @computedFrom('type')
    get text() {
        return StringHelper.prettifyEnum(StageType[this.type]);
    }
}