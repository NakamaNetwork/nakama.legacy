import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { GenericApiQueryService } from './generic/generic-query-service';

@autoinject
export class ShipQueryService extends GenericApiQueryService {
    constructor(http: HttpEngine) {
        super('ship', http, true);
    }

    getIcon(unitId: number) {
        if (unitId) {
            var id = ('0000' + unitId).slice(-4).replace(/(057[54])/, '0$1');
            return 'https://onepiece-treasurecruise.com/wp-content/uploads/f' + id + '.png';
        }
        return null;
    }
};