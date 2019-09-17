import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { IUnitStubModel, SearchConstants, IUnitSearchModel, UnitClass, UnitType, UnitFlag } from '../../models/imported';
import { SearchModel } from '../../models/search-model';
import { LocallySearchedQueryService } from './generic/locally-searched-query-service';
import { BoxService } from '../box-service';

@autoinject
export class UnitQueryService extends LocallySearchedQueryService<number, IUnitStubModel, UnitSearchModel> {
    private boxService: BoxService;

    constructor(http: HttpEngine, boxService: BoxService) {
        super('unit', http);
        this.boxService = boxService;
    }

    static getIcon(unitId: number) {
        if (unitId) {
            var id = ('0000' + unitId).slice(-4).replace(/(057[54])/, '0$1'); // missing aokiji image
            switch (id) {
                case '0742': return 'https://onepiece-treasurecruise.com/wp-content/uploads/f0742-2.png';
                case '3333': return 'http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5013.png';
                case '3334': return 'http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5014.png';
                case '3335': return 'http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5015.png';
                case '3345': return 'http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5025.png';
                case '3346': return 'http://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5026.png';
                case '3355': return 'https://onepiece-treasurecruise.com/en/wp-content/uploads/sites/2/f5037.png';
            }
            if (unitId > 3333) {
                return `/content/units/${id}.png`;
            }
            return 'https://onepiece-treasurecruise.com/wp-content/uploads/f' + id + '.png';
        }
        return null;
    }

    protected performSearch(items: IUnitStubModel[], searchModel: UnitSearchModel): IUnitStubModel[] {
        items = this.searchTerm(items, searchModel.term);
        items = this.searchTypes(items, searchModel.types);
        items = this.searchClasses(items, searchModel.classes, searchModel.forceClass);
        items = this.searchGlobal(items, searchModel.global);
        items = this.searchBox(items, searchModel.myBox);
        items = this.searchLimit(items, searchModel.limitTo);
        items = this.searchFreeToPlay(items, searchModel.freeToPlay);
        return items;
    }

    private searchTerm(items: IUnitStubModel[], term: string): IUnitStubModel[] {
        return this.doTermSearch(items, (x) => x.aliases.concat(x.name), term);
    }

    private searchTypes(items: IUnitStubModel[], unitType: UnitType): IUnitStubModel[] {
        if (unitType) {
            items = items.filter(x => (x.type & unitType) !== 0);
        }
        return items;
    }

    private searchClasses(items: IUnitStubModel[], unitClass: UnitClass, forceClass: boolean): IUnitStubModel[] {
        if (unitClass) {
            if (forceClass) {
                items = items.filter(x => x.class === unitClass);
            } else {
                items = items.filter(x => (x.class & unitClass) !== 0);
            }
        }
        return items;
    }

    private searchGlobal(items: IUnitStubModel[], global: boolean): IUnitStubModel[] {
        if (global) {
            items = items.filter(x => (x.flags & UnitFlag.Global) !== 0);
        }
        return items;
    }

    private searchBox(items: IUnitStubModel[], myBox: boolean): IUnitStubModel[] {
        if (myBox) {
            var box = this.boxService.currentBox;
            if (box) {
                var boxIds = box.unitIds;
                items = items.filter(x => boxIds.indexOf(x.id) !== -1);
            }
        }
        return items;
    }

    private searchLimit(items: IUnitStubModel[], limitTo: number[]): IUnitStubModel[] {
        if (limitTo) {
            items = items.filter(x => limitTo.indexOf(x.id) !== -1);
        }
        return items;
    }

    private searchFreeToPlay(items: IUnitStubModel[], freeToPlay: boolean): IUnitStubModel[] {
        if (freeToPlay) {
            items = items.filter(x =>
                (x.flags & UnitFlag.RareRecruitExclusive) === 0 &&
                (x.flags & UnitFlag.RareRecruitLimited) === 0);
        }
        return items;
    }

    protected performSort(items: IUnitStubModel[], sortBy: string, sortDesc: boolean): IUnitStubModel[] {
        switch (sortBy) {
            case SearchConstants.SortId:
                return this.doSort(items, [x => x.id], [sortDesc]);
            case SearchConstants.SortName:
                return this.doSort(items, [x => x.name], [sortDesc]);
            case SearchConstants.SortType:
                return this.doSort(items, [x => x.type, x => x.stars, x => x.cost], [sortDesc, true, true]);
            case SearchConstants.SortClass:
                return this.doSort(items, [x => x.class, x => x.stars, x => x.cost], [sortDesc, true, true]);
            case SearchConstants.SortStars:
                return this.doSort(items, [x => x.stars, x => x.type, x => x.cost], [sortDesc, false, true]);
            default:
                return this.doSort(items, [x => x.type, x => x.stars, x => x.cost], [false, true, true]);
        }
    }
}

export class UnitSearchModel extends SearchModel implements IUnitSearchModel {
    term: string;
    classes: UnitClass;
    types: UnitType;
    forceClass: boolean;
    myBox: boolean;
    blacklist: boolean;
    global: boolean;
    freeToPlay: boolean;
    cacheKey: string = 'search-unit';
    limitTo: number[] = null;

    sortables: string[] = [
        SearchConstants.SortId,
        SearchConstants.SortName,
        SearchConstants.SortType,
        SearchConstants.SortClass,
        SearchConstants.SortStars
    ];

    public getDefault(): UnitSearchModel {
        var model = new UnitSearchModel();
        model.sortBy = SearchConstants.SortStars;
        model.sortDesc = true;
        return model;
    }
};