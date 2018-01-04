import { autoinject, computedFrom } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { IUserProfileSearchModel } from '../../models/imported';

@autoinject
export class ProfileQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('profile', http, true);
    }

    logout() {
        return this.http.post('/account/logout');
    }
}

export class ProfileSearchModel implements IUserProfileSearchModel {
    term: string = '';
    roles: string[] = [];
    page: number = 1;
    pageSize: number = 25;

    @computedFrom('term', 'roles', 'page', 'pageSize')
    get payload() {
        var text = JSON.stringify(this);
        var output = JSON.parse(text);
        return output;
    }
}