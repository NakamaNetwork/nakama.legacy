﻿import { computedFrom } from 'aurelia-framework';
import { ISearchModel } from './imported';

export abstract class SearchModel implements ISearchModel {
    public static pageSizeKey: string = 'PageSize';

    public page: number = 1;
    public pageSize: number = SearchModel.isNumber(sessionStorage[SearchModel.pageSizeKey]) ? Number.parseInt(sessionStorage[SearchModel.pageSizeKey]) : 20;

    public totalResults: number = 0;

    abstract getDefault(): SearchModel;
    abstract getCacheKey(): string;

    getCached(): SearchModel {
        var key = this.getCacheKey();
        var json = sessionStorage[key];
        var result = null;
        if (json) {
            result = JSON.parse(json);
            this.reset(result);
        }
        return this;
    }

    get json() {
        if (this.totalResults > 0 && this.totalResults < ((this.page - 1) * this.pageSize) + 1) {
            this.page = 1;
        }
        var ified = Object.assign({}, this);
        ified.totalResults = null;
        return JSON.stringify(ified);
    }

    @computedFrom('json')
    get payload() {
        var payload = JSON.parse(this.json);
        sessionStorage.setItem(this.getCacheKey(), this.json);
        sessionStorage.setItem(SearchModel.pageSizeKey, payload.pageSize.toString());
        return payload;
    }

    reset(newItem = null) {
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
        properties.filter(x => x !== 'totalResults' && x !== 'cached').forEach(x => {
            this[x] = newItem[x];
        });
    }

    assign(params) {
        for (var myProp in params) {
            if (params.hasOwnProperty(myProp)) {
                var prop = params[myProp];
                this[myProp] = SearchModel.isNumber(prop) ? Number.parseInt(prop) : prop;
            }
        }
        return this;
    }

    static isNumber(n): boolean { return !isNaN(parseFloat(n)) && !isNaN(n - 0) }
}