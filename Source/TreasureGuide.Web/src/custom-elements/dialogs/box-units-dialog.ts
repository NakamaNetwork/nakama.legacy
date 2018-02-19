import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { BindingEngine } from 'aurelia-binding';
import { UnitQueryService, UnitSearchModel } from '../../services/query/unit-query-service';
import { IUnitStubModel, IBoxDetailModel, IBoxUnitDetailModel, IBoxUpdateModel } from '../../models/imported';
import { AlertService } from '../../services/alert-service';
import { BoxQueryService } from '../../services/query/box-query-service';
import { BoxDetailModel } from '../../services/query/box-query-service';
import { BoxService } from '../../services/box-service';

@autoinject
export class BoxUnitsDialog {
    private controller: DialogController;
    private unitQueryService: UnitQueryService;
    private boxQueryService: BoxQueryService;
    private alertService: AlertService;
    private boxService: BoxService;

    private units: IUnitStubModel[] = [];
    private searchModel: UnitSearchModel = <UnitSearchModel>new UnitSearchModel().getCached();
    private loading: boolean;
    private box: BoxDetailModel;

    constructor(unitQueryService: UnitQueryService,
        controller: DialogController,
        bindingEngine: BindingEngine,
        alertService: AlertService,
        boxQueryService: BoxQueryService,
        boxService: BoxService
    ) {
        this.controller = controller;
        this.unitQueryService = unitQueryService;
        this.alertService = alertService;
        this.boxQueryService = boxQueryService;
        this.boxService = boxService;

        this.box = <BoxDetailModel>{
            boxUnits: []
        };

        this.searchModel.boxId = null;
        bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    activate(viewModel: BoxDetailModel) {
        this.box = Object.assign(new BoxDetailModel(), viewModel);
        this.box.boxUnits = this.box.boxUnits.map(x => <IBoxUnitDetailModel>{
            unitId: x.unitId,
            name: x.name,
            flags: x.flags
        });
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

    toggle(event: CustomEvent) {
        if (event && event.detail) {
            this.boxService.toggle(event.detail.newValue.id, this.box);
        }
    }

    cancel() {
        this.controller.cancel();
    };


    submit() {
        if (this.box.dirty) {
            var model = this.box.boxUpdateModel;
            this.boxQueryService.update(model).then(x => {
                this.alertService.success('Successfully updated the box!');
                this.controller.ok(this.box);
            }).catch(response => this.alertService.reportError(response));
        } else {
            this.controller.ok(this.box);
        }
    }
}