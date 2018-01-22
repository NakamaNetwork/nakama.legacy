import { computedFrom } from 'aurelia-framework';
import { ISearchModel } from './imported';

export abstract class SearchModel implements ISearchModel {
    public static pageSizeKey: string = 'PageSize';

    public page: number = 1;
    public pageSize: number = SearchModel.isNumber(sessionStorage[SearchModel.pageSizeKey]) ? Number.parseInt(sessionStorage[SearchModel.pageSizeKey]) : 20;
    public sortBy: string;
    public sortDesc: boolean;

    public totalResults: number = 0;
    public sortables: string[] = [];
    public cacheKey: string;

    abstract getDefault(): SearchModel;

    getCached(): SearchModel {
        if (this.cacheKey) {
            var json = sessionStorage[this.cacheKey];
            var result = null;
            if (json) {
                result = JSON.parse(json);
                this.reset(result);
            }
        }
        return this;
    }

    get json() {
        if (this.totalResults > 0 && this.totalResults < ((this.page - 1) * this.pageSize) + 1) {
            this.page = 1;
        }
        var ified = Object.assign({}, this);
        ified.totalResults = undefined;
        ified.sortables = undefined;
        ified.cacheKey = undefined;
        return JSON.stringify(ified);
    }

    @computedFrom('json')
    get payload() {
        var payload = JSON.parse(this.json);
        if (this.cacheKey) {
            sessionStorage.setItem(this.cacheKey, this.json);
        }
        sessionStorage.setItem(SearchModel.pageSizeKey, payload.pageSize.toString());
        return payload;
    }

    reset(newItem = null) {
        var reset = !newItem;
        newItem = newItem || <any>this.getDefault();

        var properties = [];
        for (var myProp in this) {
            if (this.hasOwnProperty(myProp)) {
                properties.push(myProp);
            }
        }
        for (var yourProp in newItem) {
            if (newItem.hasOwnProperty(yourProp)) {
                properties.push(yourProp);
            }
        }
        properties.filter(x => x !== 'totalResults' && x !== 'cached' && x !== 'sortables' && x !== 'cacheKey').forEach(x => {
            var value = newItem[x];
            if (reset || value !== undefined) {
                this[x] = newItem[x];
            }
        });
    }

    assign(params) {
        var success = false;
        if (params) {
            for (var myProp in params) {
                if (params.hasOwnProperty(myProp)) {
                    success = true;
                    var prop = params[myProp];
                    this[myProp] = SearchModel.isNumber(prop) ? Number.parseInt(prop) : prop;
                }
            }
        }
        return success;
    }

    static isNumber(n): boolean { return !isNaN(parseFloat(n)) && !isNaN(n - 0) }
}