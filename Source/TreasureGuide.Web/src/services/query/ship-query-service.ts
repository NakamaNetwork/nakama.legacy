import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { IShipStubModel, ISearchModel } from '../../models/imported';
import { SearchModel } from '../../models/search-model';
import { LocallySearchedQueryService } from './generic/locally-searched-query-service';

@autoinject
export class ShipQueryService extends LocallySearchedQueryService<number, IShipStubModel, ShipSearchModel> {
    constructor(http: HttpEngine) {
        super('ship', http);
    }

    static getIcon(unitId: number) {
        if (unitId) {
            var id = ('0000' + unitId).slice(-4).replace(/(057[54])/, '0$1');
            return 'https://onepiece-treasurecruise.com/wp-content/uploads/f' + id + '.png';
        }
        return null;
    }

    public performSearch(items: IShipStubModel[], searchModel: ShipSearchModel): IShipStubModel[] {
        return items;
    }

    public performSort(items: IShipStubModel[], sortBy: string, sortDesc: boolean): IShipStubModel[] {
        return items;
    }
};

export class ShipSearchModel extends SearchModel implements ISearchModel {
    getDefault(): SearchModel {
        return new ShipSearchModel();
    }
}