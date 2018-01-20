import { bindable, customElement } from 'aurelia-framework';
import { StageSearchModel } from "../services/query/stage-query-service";

@customElement('stage-search')
export class StageSearch {
    @bindable
    searchModel: StageSearchModel;
}