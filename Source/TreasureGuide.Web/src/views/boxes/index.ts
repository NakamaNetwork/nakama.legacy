import { autoinject, BindingEngine } from 'aurelia-framework';
import { BoxSearchModel, BoxQueryService } from '../../services/query/box-query-service';
import { IBoxStubModel } from '../../models/imported';
import { BoxService } from '../../services/box-service';

@autoinject
export class BoxIndexPage {
    private boxService: BoxService;
    private boxQueryService: BoxQueryService;
    private bindingEngine: BindingEngine;

    title: string = 'My Boxes';
    searchModel: BoxSearchModel;
    boxes: IBoxStubModel[] = [];
    loading: boolean;

    constructor(boxService: BoxService, boxQueryService: BoxQueryService, bindingEngine: BindingEngine) {
        this.boxService = boxService;
        this.boxQueryService = boxQueryService;
        this.bindingEngine = bindingEngine;
    }

    activate() {
        this.searchModel = new BoxSearchModel().getDefault();
        this.bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    clicked(box: IBoxStubModel) {
        if (this.boxService.currentBox && this.boxService.currentBox.id === box.id) {
            this.boxService.setBox(null);
        } else {
            this.boxService.setBox(box.id);
        }
    }

    search(payload) {
        if (this.boxQueryService) {
            this.loading = true;
            this.boxQueryService.search(payload).then(x => {
                this.boxes = x.results;
                this.searchModel.totalResults = x.totalResults;
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }
}