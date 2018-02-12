import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { BindingEngine } from 'aurelia-binding';
import { UnitQueryService, UnitSearchModel } from '../../services/query/unit-query-service';
import { IUnitStubModel, IBoxDetailModel, IBoxUpdateModel } from '../../models/imported';
import { AlertService } from '../../services/alert-service';
import { BoxQueryService } from '../../services/query/box-query-service';

@autoinject
export class BoxUnitsDialog {
    private controller: DialogController;
    private unitQueryService: UnitQueryService;
    private boxQueryService: BoxQueryService;
    private alertService: AlertService;

    private units: IUnitStubModel[] = [];
    private searchModel: UnitSearchModel = <UnitSearchModel>new UnitSearchModel().getCached();
    private loading: boolean;
    private box: IBoxDetailModel;

    constructor(unitQueryService: UnitQueryService,
        controller: DialogController,
        bindingEngine: BindingEngine,
        alertService: AlertService,
        boxQueryService: BoxQueryService) {
        this.controller = controller;
        this.unitQueryService = unitQueryService;
        this.alertService = alertService;
        this.boxQueryService = boxQueryService;

        this.box = <IBoxDetailModel>{
            unitIds: []
        };

        this.searchModel.boxId = null;
        bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    activate(viewModel: IBoxDetailModel) {
        this.box = Object.assign({}, viewModel);
        this.box.unitIds = this.box.unitIds.map(x => x);
    }

    search(payload: UnitSearchModel) {
        if (this.unitQueryService) {
            this.loading = true;
            this.unitQueryService.search(payload).then(x => {
                this.units = x.results;
                this.searchModel.totalResults = x.totalResults;
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }

    cancel() {
        this.controller.cancel();
    };


    submit() {
        var model = <IBoxUpdateModel>{
            id: this.box.id,
            added: this.box.unitIds
        };
        this.boxQueryService.set(model).then(x => {
            this.alertService.success('Successfully updated the box!');
            this.controller.ok(this.box);
        }).catch(response => this.alertService.reportError(response));
    }
}