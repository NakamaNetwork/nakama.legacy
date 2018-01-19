import { GenericApiQueryService } from './generic-api-query-service';
import { HttpEngine } from '../../../tools/http-engine';

export class SearchableQueryService extends GenericApiQueryService {
    constructor(controller: string, http: HttpEngine, cached?: boolean) {
        super(controller, http, cached);
    }

    private timeout;

    search(model) {
        if (this.timeout) {
            clearTimeout(this.timeout);
        }
        var promise = new Promise<any>((resolve, reject) => {
            this.timeout = setTimeout(() => {
                var endpoint = this.buildAddress('search');
                endpoint = this.http.parameterize(endpoint, model);
                resolve(this.http.get(endpoint));
            }, 100);
        });
        return promise;
    }
}