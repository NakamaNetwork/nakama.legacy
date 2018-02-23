import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { SearchModel } from '../models/search-model';

@customElement('search-pager')
export class SearchPager {
    @bindable searchModel: SearchModel;
    @bindable bottom = false;
    pageSizes = [10, 20, 50, 100];

    @bindable emptyMessage = 'Search yielded no results!';

    @computedFrom('searchModel.page', 'searchModel.pageSize', 'searchModel.totalResults')
    get startIndex() {
        return ((this.searchModel.page - 1) * this.searchModel.pageSize) + 1;
    }

    @computedFrom('searchModel.page', 'searchModel.pageSize', 'searchModel.totalResults')
    get endIndex() {
        return Math.min(this.searchModel.totalResults, this.searchModel.page * this.searchModel.pageSize);
    }

    @computedFrom('searchModel.totalResults')
    get hasResults() {
        return this.searchModel.totalResults > 0;
    }

    @computedFrom('searchModel.pageSize', 'searchModel.totalResults')
    get hasPages() {
        return this.searchModel.totalResults > this.searchModel.pageSize;
    }
}