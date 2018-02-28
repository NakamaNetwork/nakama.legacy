import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchModel } from '../../models/search-model';
import { IStageStubModel, IStageSearchModel, SearchConstants } from '../../models/imported';
import { LocallySearchedQueryService } from './generic/locally-searched-query-service';

@autoinject
export class StageQueryService extends LocallySearchedQueryService<number, IStageStubModel, StageSearchModel> {
    constructor(http: HttpEngine) {
        super('stage', http);
    }

    protected performSearch(items: IStageStubModel[], searchModel: StageSearchModel): IStageStubModel[] {
        return items;
    }

    protected performSort(items: IStageStubModel[], sortBy: string, sortDesc: boolean): IStageStubModel[] {
        return items;
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