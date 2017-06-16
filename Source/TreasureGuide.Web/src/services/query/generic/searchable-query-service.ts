import { GenericApiQueryService } from './generic-query-service';
import { HttpEngine } from '../../../tools/http-engine';

export class SearchableQueryService extends GenericApiQueryService {
    constructor(controller: string, http: HttpEngine, cached?: boolean) {
        super(controller, http, cached);
    }
    
    search(model) {
        var endpoint = this.buildAddress('search');
        endpoint = this.http.parameterize(endpoint, model);
        return this.http.get(endpoint);
    }
}