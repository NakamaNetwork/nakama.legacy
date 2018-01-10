import { computedFrom } from 'aurelia-framework';
import { ISearchModel } from './imported';

export abstract class SearchModel implements ISearchModel {
    public page: number = 1;
    public pageSize: number = 25;

    public totalResults: number = 0;
    public cached: boolean = true;

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
        var ified = Object.assign({}, this);
        ified.cached = null;
        ified.totalResults = null;
        return JSON.stringify(ified);
    }

    @computedFrom('json')
    get payload() {
        if (this.cached) {
            var key = this.constructor.name;
            sessionStorage[key] = this.json;
        }
        return JSON.parse(this.json);
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
}