import { computedFrom } from 'aurelia-framework';
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

export class UnitSearchModel extends SearchModel implements IUnitSearchModel {
    term: string;
    classes: number[];
    types: number[];
    forceTypes: boolean;
    myBox: boolean;
    global: boolean;
    freeToPlay: boolean;

    public getDefault(): SearchModel {
        return new UnitSearchModel();
    }
};