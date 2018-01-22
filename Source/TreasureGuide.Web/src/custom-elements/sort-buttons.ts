import { customElement, bindable } from "aurelia-framework";
import { ISearchModel } from '../models/imported';

@customElement('sort-buttons')
export class SortButtons {
    @bindable searchModel: ISearchModel;

    click(type: string) {
        if (this.searchModel.sortBy === type) {
            if (this.searchModel.sortDesc) {
                this.searchModel.sortDesc = undefined;
                this.searchModel.sortBy = undefined;
            } else {
                this.searchModel.sortDesc = true;
            }
        } else {
            this.searchModel.sortBy = type;
            this.searchModel.sortDesc = false;
        }
    }
}