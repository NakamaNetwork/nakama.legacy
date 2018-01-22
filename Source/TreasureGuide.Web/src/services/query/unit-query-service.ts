import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { IUnitSearchModel } from '../../models/imported';
import { SearchModel } from '../../models/search-model';

@autoinject
export class UnitQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('unit', http, true);
    }

    static getIcon(unitId: number) {
        if (unitId) {
            var id = ('0000' + unitId).slice(-4).replace(/(057[54])/, '0$1'); // missing aokiji image
            if (id == '0742')
                return 'https://onepiece-treasurecruise.com/wp-content/uploads/f0742-2.png';
            if (id == '1923')
                return 'http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5009.png';
            if (id == '1924')
                return 'http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5010.png';
            return 'https://onepiece-treasurecruise.com/wp-content/uploads/f' + id + '.png';
        }
        return null;
    }
}

export class UnitSearchModel extends SearchModel implements IUnitSearchModel {

    term: string;
    classes: number[];
    types: number[];
    forceClass: boolean;
    myBox: boolean;
    global: boolean;
    freeToPlay: boolean;
    cacheKey: string = 'search-unit';

    public getDefault(): SearchModel {
        return new UnitSearchModel();
    }
};