import { HttpEngine } from '../../../tools/http-engine';
import { CacheItem } from '../../../models/cache-item';
import { LocallyCachedQueryService } from './locally-cached-query-service';
import { SearchModel, SearchResult } from '../../../models/search-model';
import * as firstBy from 'thenby';

export abstract class LocallySearchedQueryService<TId, TEntity extends CacheItem, TSearchModel extends SearchModel>
    extends LocallyCachedQueryService<TId, TEntity> {

    constructor(controller: string, http: HttpEngine) {
        super(controller, http);
    }

    public search(model: TSearchModel): Promise<SearchResult<TEntity>> {
        var searchCallback = (x: TEntity[]) => {
            var results = this.performSearch(x, model);
            var count = results.length;
            results = this.performSort(results, model.sortBy, model.sortDesc);
            results = this.performPagination(results, model.page, model.pageSize);

            var resultModel = new SearchResult<TEntity>();
            resultModel.results = results;
            resultModel.totalResults = count;
            return resultModel;
        }
        return this.get().then(searchCallback);
    }

    protected abstract performSearch(items: TEntity[], searchModel: TSearchModel): TEntity[];
    protected abstract performSort(items: TEntity[], sortBy: string, sortDesc: boolean): TEntity[];

    protected performPagination(items: TEntity[], page: number, pageSize: number): TEntity[] {
        var start = (page - 1) * pageSize;
        return items.slice(start, start + pageSize);
    }

    protected doTermSearch(items: TEntity[], selector: (x: TEntity) => string[], criteria: string) {
        criteria = (criteria || '').trim();
        if (criteria) {
            var allTerms = criteria.toLowerCase().split(' ');
            items = items.filter(x => selector(x).some(y => allTerms.some(z => y.indexOf(z.toLowerCase()) !== -1)));
        }
        return items;
    }

    protected doSort(items: TEntity[], selector: ((x) => any[])[], desc: boolean[]) {
        var sorter;
        var dirs = desc.map(x => x ? -1 : 1);
        for (var i = 0; i < selector.length; i++) {
            if (i === 0) {
                sorter = firstBy(selector[i], dirs[i]);
            } else {
                sorter = sorter.thenBy(selector[i], dirs[i]);
            }
        }
        return items.sort(sorter);
    }
}