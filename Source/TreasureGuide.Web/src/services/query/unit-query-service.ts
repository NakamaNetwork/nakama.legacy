import { autoinject } from 'aurelia-dependency-injection';
import { GenericApiQueryService } from './generic/generic-api-query-service';
import { HttpEngine } from '../../tools/http-engine';

@autoinject
export class UnitQueryService extends GenericApiQueryService {
    constructor(http: HttpEngine) {
        super('unit', http);
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
}