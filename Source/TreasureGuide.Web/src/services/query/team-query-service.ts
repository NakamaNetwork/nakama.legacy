import { computedFrom } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';

@autoinject
export class TeamQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('team', http);
    }
}

export class TeamSearchModel {
    term?: string;
    leaderId?: number;
    stageId?: number;
    myBox?: boolean = false;
    global?: boolean = false;
    freeToPlay?: boolean = false;
    page?: number = 1;
    pageSize?: number = 25;

    @computedFrom('term', 'leaderId', 'stageId', 'myBox', 'global', 'freeToPlay', 'page', 'pageSize')
    get payload() {
        var text = JSON.stringify(this);
        var output = JSON.parse(text);
        return output;
    }
};