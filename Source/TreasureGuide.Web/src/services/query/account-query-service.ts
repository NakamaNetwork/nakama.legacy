import { computedFrom } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';

@autoinject
export class AccountQueryService {
    private http: HttpEngine;

    constructor(http: HttpEngine) {
        this.http = http;
    }

    getExternalLoginProviders() {
        return this.http.get('/account/GetExternalLoginProviders');
    }

    login(provider) {
        return this.http.post('/account/externalLogin', { provider: provider, returnUrl: '/' });
    }
}