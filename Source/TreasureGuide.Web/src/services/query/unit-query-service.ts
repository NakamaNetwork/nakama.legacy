import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { IUnitStubModel, SearchConstants, IUnitSearchModel, UnitClass, UnitType } from '../../models/imported';
import { SearchModel } from '../../models/search-model';
import { LocallySearchedQueryService } from './generic/locally-searched-query-service';

@autoinject
export class UnitQueryService extends LocallySearchedQueryService<number, IUnitStubModel, UnitSearchModel> {
    constructor(http: HttpEngine) {
        super('unit', http);
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

    public performSearch(items: IUnitStubModel[], searchModel: UnitSearchModel): IUnitStubModel[] {
        return items;
    }

    public performSort(items: IUnitStubModel[], sortBy: string, sortDesc: boolean): IUnitStubModel[] {
        return items;
    }
}

export class UnitSearchModel extends SearchModel implements IUnitSearchModel {
    term: string;
    classes: UnitClass;
    types: UnitType;
    forceClass: boolean;
    boxId: number;
    blacklist: boolean;
    global: boolean;
    freeToPlay: boolean;
    cacheKey: string = 'search-unit';


    sortables: string[] = [
        SearchConstants.SortId,
        SearchConstants.SortName,
        SearchConstants.SortType,
        SearchConstants.SortClass,
        SearchConstants.SortStars
    ];

    public getDefault(): SearchModel {
        return new UnitSearchModel();
    }
};