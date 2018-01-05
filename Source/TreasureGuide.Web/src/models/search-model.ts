import { computedFrom } from 'aurelia-framework';
import { ISearchModel } from './imported';

export abstract class SearchModel implements ISearchModel {
    public page: number = 1;
    public pageSize: number = 25;

    abstract getDefault(): SearchModel;

    getCached(): SearchModel {
        var key = this.constructor.name;
        var json = localStorage[key];
        var result = null;
        if (json) {
            result = JSON.parse(json);
            this.reset(result);
        }
        return this;
    }

    get json() {
        return JSON.stringify(this);
    }

    @computedFrom('json')
    get payload() {
        var key = this.constructor.name;
        localStorage[key] = this.json;
        return JSON.parse(this.json);
    }

    reset(newItem = null) {
        newItem = newItem || <any>this.getDefault();
        for (var myProp in this) {
            if (this.hasOwnProperty(myProp)) {
                this[myProp] = newItem[myProp];
            }
        }
        for (var yourProp in newItem) {
            if (newItem.hasOwnProperty(yourProp)) {
                this[yourProp] = newItem[yourProp];
            }
        }
    }
}