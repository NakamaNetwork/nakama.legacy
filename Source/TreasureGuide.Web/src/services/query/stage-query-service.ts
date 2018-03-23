import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchModel } from '../../models/search-model';
import { IStageStubModel, IStageSearchModel, SearchConstants, StageType } from '../../models/imported';
import { LocallySearchedQueryService } from './generic/locally-searched-query-service';
import * as moment from 'moment';
import { NumberHelper } from '../../tools/number-helper';

@autoinject
export class StageQueryService extends LocallySearchedQueryService<number, IStageStubModel, StageSearchModel> {
    constructor(http: HttpEngine) {
        super('stage', http);
    }

    protected performSearch(items: IStageStubModel[], searchModel: StageSearchModel): IStageStubModel[] {
        items = this.searchTerm(items, searchModel.term);
        items = this.searchType(items, searchModel.type);
        items = this.searchGlobal(items, searchModel.global);
        return items;
    }

    protected searchTerm(items: IStageStubModel[], term: string): IStageStubModel[] {
        return this.doTermSearch(items, x => x.aliases.concat(x.name), term);
    }

    protected searchType(items: IStageStubModel[], type: StageType): IStageStubModel[] {
        if (type) {
            items = items.filter(x => x.type === type);
        }
        return items;
    }

    protected searchGlobal(items: IStageStubModel[], global: boolean): IStageStubModel[] {
        if (global) {
            items = items.filter(x => x.global);
        }
        return items;
    }

    protected performSort(items: IStageStubModel[], sortBy: string, sortDesc: boolean): IStageStubModel[] {
        switch (sortBy) {
            case SearchConstants.SortName:
                return this.doSort(items, [x => x.name, x => x.unitId], [sortDesc, false]);
            case SearchConstants.SortType:
                return this.doSort(items, [x => x.type, x => x.name], [sortDesc, false]);
            case SearchConstants.SortCount:
                return this.doSort(items, [x => x.teamCount, x => x.name, x => x.unitId], [sortDesc, false, false]);
            default:
                return this.doSort(items, [x => x.name, x => x.unitId], [false]);
        }
    }

    private scheduleDateKey: string = 'controller_cache_date_stage_schedule';
    private scheduleKey: string = 'controller_cache_stage_schedule';

    public schedule() {
        var lastChecked = localStorage.getItem(this.scheduleDateKey);
        var now = moment.utc();
        var then = moment.unix(NumberHelper.forceNumber(lastChecked));
        if (lastChecked && now.isAfter(then)) {
            var scheduleJson = localStorage.getItem(this.scheduleKey);
            if (scheduleJson) {
                var decoded = JSON.parse(scheduleJson);
                if (decoded) {
                    return new Promise((resolve) => {
                        resolve(decoded);
                    });
                }
            }
        }
        then = moment.utc().startOf('day').add(3, 'hours');
        if (now.hours() >= 3) {
            then = then.add(1, 'days');
        }
        return this.http.get(this.buildAddress('schedule')).then(x => {
            localStorage.setItem(this.scheduleDateKey, JSON.stringify(then.unix()));
            localStorage.setItem(this.scheduleKey, JSON.stringify(x));
            return x;
        });
    }
}

export class StageSearchModel extends SearchModel implements IStageSearchModel {
    term: string = '';
    type: StageType;
    global: boolean = false;
    cacheKey: string = 'search-stage';

    public sortables: string[] = [
        SearchConstants.SortName,
        SearchConstants.SortType,
        SearchConstants.SortCount
    ];

    getDefault(): SearchModel {
        return new StageSearchModel();
    }
}