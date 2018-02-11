import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { IBoxEditorModel, IBoxSearchModel, IBoxUpdateModel } from '../../models/imported';
import { SearchModel } from '../../models/search-model';
import { IBoxDetailModel } from '../../models/imported';

@autoinject
export class BoxQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('box', http, true);
    }

    update(model: IBoxUpdateModel): Promise<any> {
        return this.http.post(this.buildAddress('update'), model);
    }

    set(model: IBoxUpdateModel): Promise<any> {
        return this.http.post(this.buildAddress('set'), model);
    }

    focus(id: number): Promise<IBoxDetailModel> {
        return this.http.post(this.buildAddress('focus/' + id));
    }
}

export class BoxSearchModel extends SearchModel implements IBoxSearchModel {
    userId: string;
    blacklist: boolean;

    public getDefault(): SearchModel {
        return new BoxSearchModel();
    }
}

export class BoxEditorModel implements IBoxEditorModel {
    id: number;
    name: string;
    friendId: number;
    global: boolean = false;
    public: boolean = true;
    blacklist: boolean = false;

    deleted: boolean = false;
}