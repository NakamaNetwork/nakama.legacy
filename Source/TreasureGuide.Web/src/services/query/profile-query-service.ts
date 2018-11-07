import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { IProfileSearchModel } from '../../models/imported';
import { IProfileEditorModel } from '../../models/imported';
import { SearchModel } from '../../models/search-model';
import { SearchConstants } from '../../models/imported';

@autoinject
export class ProfileQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('profile', http);
    }

    logout() {
        return this.http.post('/account/logout');
    }
}

export class ProfileSearchModel extends SearchModel implements IProfileSearchModel {
    term: string = '';
    roles: string[] = [];
    cacheKey: string = 'search-profile';

    public getDefault(): ProfileSearchModel {
        var model = new ProfileSearchModel();
        model.sortBy = SearchConstants.SortName;
        model.sortDesc = false;
        return model;
    }
}

export class ProfileEditorModel implements IProfileEditorModel {
    public id: string;
    public userName: string;
    public unitId: number;
    public friendId: number;
    public website: string;
    public global: boolean;
    public userRoles: string[];
}