import { autoinject, computedFrom } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { IProfileSearchModel } from '../../models/imported';
import { IProfileEditorModel } from '../../models/imported';
import { SearchModel } from '../../models/search-model';

@autoinject
export class ProfileQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('profile', http, true);
    }

    logout() {
        return this.http.post('/account/logout');
    }
}

export class ProfileSearchModel extends SearchModel implements IProfileSearchModel {
    term: string = '';
    roles: string[] = [];

    public getDefault(): SearchModel {
        return new ProfileSearchModel();
    }

    getCacheKey(): string {
        return 'search-profile';
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