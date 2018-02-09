import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { IBoxSearchModel } from '../../models/imported';
import { SearchModel } from '../../models/search-model';

@autoinject
export class BoxQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('box', http, true);
    }
}

export class BoxSearchModel extends SearchModel implements IBoxSearchModel {
    userId: string;
    blacklist: boolean;

    public getDefault(): SearchModel {
        return new BoxSearchModel();
    }
}