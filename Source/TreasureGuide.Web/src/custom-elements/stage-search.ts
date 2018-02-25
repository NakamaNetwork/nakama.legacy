import { bindable, customElement } from 'aurelia-framework';
import { StageSearchModel } from "../services/query/stage-query-service";

@customElement('stage-search')
export class StageSearch {
    @bindable
    searchModel: StageSearchModel;

    @bindable
    termLocked: boolean;

    @bindable
    typeLocked: boolean;

    bind() {
        if (this.searchModel) {
            if (this.termLocked) {
                this.searchModel.lockedFields.push('term');
            }
            if (this.typeLocked) {
                this.searchModel.lockedFields.push('type');
            }
        }
    }
}