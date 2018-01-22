import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { IStageSearchModel } from '../../models/imported';
import { SearchModel } from '../../models/search-model';
import { SearchConstants } from '../../models/imported';

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
    cacheKey: string = 'search-stage';

    public sortables: string[] = [
        SearchConstants.SortName,
        SearchConstants.SortType,
        SearchConstants.SortCount
    ];

    getDefault(): SearchModel {
        return new StageSearchModel();
    }
}