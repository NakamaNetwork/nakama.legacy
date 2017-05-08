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
    team?: string;
    leaderId?: number;
    stageId?: number;
    myBox?: boolean = false;
    global?: boolean = false;
    page?: number = 0;
    pageSize?: number = 25;
};