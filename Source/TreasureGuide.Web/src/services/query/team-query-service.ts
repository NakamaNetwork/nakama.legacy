import { computedFrom } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { GenericApiQueryService } from './generic/generic-api-query-service';
import { HttpEngine } from '../../tools/http-engine';

@autoinject
export class TeamQueryService extends GenericApiQueryService {
    constructor(http: HttpEngine) {
        super('team', http);
    }

    search(model: TeamSearchModel) {
        var endpoint = this.http.buildEndpoint('api/team/search', model);
        return this.http.get(endpoint);
    }
}

export class TeamSearchModel {
    term?: string;
    leaderId?: number;
    stageId?: number;
    myBox?: boolean = false;
    global?: boolean = false;
    page?: number = 1;
    pageSize?: number = 25;

    @computedFrom('term', 'leaderId', 'stageId', 'myBox', 'global', 'page', 'pageSize')
    get payload() {
        var text = JSON.stringify(this);
        var output = JSON.parse(text);
        return output;
    }
};