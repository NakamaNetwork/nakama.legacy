import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { IStageSearchModel } from '../../models/imported';
import { SearchModel } from '../../models/search-model';

@autoinject
export class StageQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('stage', http, true);
    }
}

export class StageSearchModel extends SearchModel implements IStageSearchModel {
    term: string = '';
    type: number;
    global: boolean = false;

    getDefault(): SearchModel {
        return new StageSearchModel();
    }
}