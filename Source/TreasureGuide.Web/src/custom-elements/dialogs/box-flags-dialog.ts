import { autoinject, computedFrom } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { BindingEngine } from 'aurelia-binding';
import { UnitQueryService, UnitSearchModel } from '../../services/query/unit-query-service';
import { IUnitStubModel, IBoxUnitDetailModel, IBoxUpdateModel, IBoxUnitUpdateModel } from '../../models/imported';
import { AlertService } from '../../services/alert-service';
import { BoxQueryService, BoxDetailModel } from '../../services/query/box-query-service';

@autoinject
export class BoxFlagsDialog {
    private controller: DialogController;
    private unitQueryService: UnitQueryService;
    private boxQueryService: BoxQueryService;
    private alertService: AlertService;
    private bindingEngine: BindingEngine;

    private units: IUnitStubModel[] = [];
    private searchModel: UnitSearchModel = <UnitSearchModel>new UnitSearchModel().getCached();
    private loading: boolean;
    private box: BoxDetailModel;

    constructor(unitQueryService: UnitQueryService,
        controller: DialogController,
        bindingEngine: BindingEngine,
        alertService: AlertService,
        boxQueryService: BoxQueryService
    ) {
        this.controller = controller;
        this.unitQueryService = unitQueryService;
        this.alertService = alertService;
        this.boxQueryService = boxQueryService;
        this.bindingEngine = bindingEngine;

        this.box = <BoxDetailModel>{
            boxUnits: []
        };

    }

    activate(viewModel: BoxDetailModel) {
        this.box = Object.assign(new BoxDetailModel(), viewModel);
        this.box.boxUnits = this.box.boxUnits.map(x => <IBoxUnitDetailModel>{
            unitId: x.unitId,
            flags: x.flags
        });

        this.searchModel.myBox = false;
        this.searchModel.limitTo = this.box.unitIds;

        this.bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
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

    updateFlags(unitId: number, event: CustomEvent) {
        this.box.update(unitId, event.detail.newValue);
    }

    @computedFrom('units', 'box')
    get mappedUnits() {
        return this.units.map(x => {
            x['boxUnit'] = this.box.boxUnits.find(y => y.unitId === x.id);
            return x;
        });
    }

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

    cancel() {
        this.controller.cancel();
    };
}