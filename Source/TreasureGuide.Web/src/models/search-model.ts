import { computedFrom } from 'aurelia-framework';
import { ISearchModel } from './imported';

export abstract class SearchModel implements ISearchModel {
    public page: number = 1;
    public pageSize: number = 25;

    abstract getDefault(): SearchModel;

    get json() {
        return JSON.stringify(this);
    }

    @computedFrom('json')
    get payload() {
        return JSON.parse(this.json);
    }

    reset() {
        var newItem = <any>this.getDefault();
        for (var property in this) {
            if (this.hasOwnProperty(property)) {
                this[property] = newItem[property];
            }
        }
    }
}