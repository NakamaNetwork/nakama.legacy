import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { IMetaResultModel } from '../../models/imported';

@autoinject
export class MetaQueryService {
    private http: HttpEngine;

    constructor(http: HttpEngine) {
        this.http = http;
    }

    get(location): Promise<IMetaResultModel> {
        return this.http.get('/api/meta?id=' + encodeURIComponent(location));
    }
}