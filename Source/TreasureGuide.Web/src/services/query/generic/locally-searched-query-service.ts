import { HttpEngine } from '../../../tools/http-engine';
import { CacheItem } from '../../../models/cache-item';
import { LocallyCachedQueryService } from './locally-cached-query-service';
import { SearchModel } from '../../../models/search-model';
import { SearchResult } from '../../../models/search-model';

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
}