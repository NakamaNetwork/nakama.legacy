import { GenericApiQueryService } from './generic-query-service';
import { HttpEngine } from '../../../tools/http-engine';

export class SearchableQueryService extends GenericApiQueryService {
    constructor(controller: string, http: HttpEngine, cached?: boolean) {
        super(controller, http, cached);
    }

    getIcon(unitId: number) {
        if (unitId) {
            var id = ('0000' + unitId).slice(-4).replace(/(057[54])/, '0$1');
            return 'https://onepiece-treasurecruise.com/wp-content/uploads/f' + id + '.png';
        }
        return null;
    }

    getPortrait(unitId: number) {
        if (unitId) {
            var id = ('0000' + unitId).slice(-4).replace(/(057[54])/, '0$1');
            return 'https://onepiece-treasurecruise.com/wp-content/uploads/c' + id + '.png';
        }
        return null;
    }

    search(model) {
        var endpoint = this.buildAddress('search');
        endpoint = this.http.parameterize(endpoint, model);
        return this.http.get(endpoint);
    }
}