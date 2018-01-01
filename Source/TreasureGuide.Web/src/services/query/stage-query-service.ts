import { autoinject } from 'aurelia-framework';
import { computedFrom } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import {IStageSearchModel} from '../../models/imported';

@autoinject
export class StageQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('stage', http, true);
    }
}

export class StageSearchModel implements IStageSearchModel {
    term: string = '';
    type: number;
    global: boolean = false;
    page: number = 1;
    pageSize: number = 25;

    @computedFrom('term', 'type', 'global', 'page', 'pageSize')
    get payload() {
        var text = JSON.stringify(this);
        var output = JSON.parse(text);
        return output;
    }
}