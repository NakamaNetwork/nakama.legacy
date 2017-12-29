import { bindable, computedFrom, customElement } from 'aurelia-framework';
import { ISearchModel } from '../models/imported';

@customElement('search-pager')
export class SearchPager {
    @bindable searchModel: ISearchModel;
    @bindable resultCount = 0;
    pageSizes = [10, 25, 50, 100];

    @computedFrom('searchModel.page', 'searchModel.pageSize', 'resultCount')
    get startIndex() {
        return ((this.searchModel.page - 1) * this.searchModel.pageSize) + 1;
    }

    @computedFrom('searchModel.page', 'searchModel.pageSize', 'resultCount')
    get endIndex() {
        return Math.min(this.resultCount, this.searchModel.page * this.searchModel.pageSize);
    }

    @computedFrom('resultCount')
    get hasResults() {
        return this.resultCount > 0;
    }

    @computedFrom('searchModel.pageSize', 'resultCount')
    get hasPages() {
        return this.resultCount > this.searchModel.pageSize;
    }

    resultCountChanged() {
        if (this.startIndex > this.endIndex && this.resultCount !== 0) {
            this.searchModel.page = 1;
        }
    }
}