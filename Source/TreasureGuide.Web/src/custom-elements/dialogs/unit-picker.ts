import { autoinject, bindable } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { BindingEngine } from 'aurelia-binding';
import { UnitQueryService, UnitSearchModel } from '../../services/query/unit-query-service';
import { IUnitEditorModel } from '../../models/imported';
import { ITeamGenericSlotEditorModel } from '../../models/imported';
import { IUnitStubModel } from '../../models/imported';

@autoinject
export class UnitPicker {
    private controller: DialogController;
    private unitQueryService: UnitQueryService;

    private allowGenerics: boolean;
    private generic: boolean;
    private units: IUnitStubModel[] = [];
    private searchModel = new UnitSearchModel().getCached();
    private loading: boolean;

    private genericBuilder: ITeamGenericSlotEditorModel = <ITeamGenericSlotEditorModel>{};

    constructor(unitQueryService: UnitQueryService, controller: DialogController, bindingEngine: BindingEngine) {
        this.controller = controller;
        this.unitQueryService = unitQueryService;

        bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    activate(viewModel: UnitPickerParams) {
        this.allowGenerics = viewModel.allowGenerics;
        if (this.allowGenerics && viewModel.model && !viewModel.model.unitId) {
            this.generic = true;
            this.genericBuilder = viewModel.model;
        }
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

    showGeneric() {
        this.generic = true;
    }

    showUnits() {
        this.generic = false;
    }

    cancel() {
        this.controller.cancel();
    };

    clicked(model) {
        this.controller.ok(model);
    }

    getIcon(id: number) {
        return UnitQueryService.getIcon(id);
    }
}

export class UnitPickerParams {
    model: any;
    allowGenerics: boolean;
}