import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { IGCRResultModel } from '../../models/imported';

@autoinject
export class GCRQueryService {
    private http: HttpEngine;

    constructor(http: HttpEngine) {
        this.http = http;
    }

    gcrTable(): Promise<IGCRResultModel> {
        return this.http.get('/api/gcr');
    }
}