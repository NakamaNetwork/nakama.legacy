import { computedFrom } from 'aurelia-framework';
import { autoinject } from 'aurelia-dependency-injection';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';

@autoinject
export class UnitQueryService extends SearchableQueryService {
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

export class UnitSearchModel {
    term?: string;
    classes?: number[];
    types?: number[];
    forceTypes?: boolean = false;
    myBox?: boolean = false;
    global?: boolean = false;
    page?: number = 1;
    pageSize?: number = 25;

    @computedFrom('term', 'classes', 'types', 'forceTypes', 'myBox', 'global', 'page', 'pageSize')
    get payload() {
        var text = JSON.stringify(this);
        var output = JSON.parse(text);
        return output;
    }
};