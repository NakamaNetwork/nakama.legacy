import { autoinject } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { BindingEngine } from 'aurelia-binding';
import { UnitQueryService, UnitSearchModel } from '../../services/query/unit-query-service';
import { IUnitStubModel } from '../../models/imported';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import { AlertService } from '../../services/alert-service';
import { TeamGenericSlotEditorModel } from '../../services/query/team-query-service';

@autoinject
export class UnitPicker {
    private controller: DialogController;
    private unitQueryService: UnitQueryService;
    private validController: ValidationController;
    private alert: AlertService;

    private allowGenerics: boolean;
    private generic: boolean;
    private units: IUnitStubModel[] = [];
    private searchModel = new UnitSearchModel().getCached();
    private loading: boolean;

    private genericBuilder: TeamGenericSlotEditorModel = new TeamGenericSlotEditorModel();

    constructor(unitQueryService: UnitQueryService,
        controller: DialogController,
        bindingEngine: BindingEngine,
        validFactory: ValidationControllerFactory,
        alertService: AlertService) {
        this.alert = alertService;
        this.controller = controller;
        this.unitQueryService = unitQueryService;

        this.validController = validFactory.createForCurrentScope();
        this.validController.addRenderer(new BeauterValidationFormRenderer());
        this.validController.addObject(this.genericBuilder);

        bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    activate(viewModel: UnitPickerParams) {
        this.allowGenerics = viewModel.allowGenerics;
        if (this.allowGenerics && viewModel.model && !viewModel.model.unitId) {
            this.generic = true;
            Object.assign(this.genericBuilder, viewModel.model);
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

    eventClicked(event: CustomEvent) {
        if (event && event.detail) {
            this.clicked(event.detail.newValue);
        } else {
            this.clicked(null);
        }
    }

    clicked(model) {
        if (model && !model.id) {
            this.validController.validate().then(x => {
                if (x.valid) {
                    this.controller.ok(model);
                } else {
                    x.results.filter(y => !y.valid && y.message).forEach(y => {
                        this.alert.danger(y.message);
                    });
                }
            });
        } else {
            this.controller.ok(model);
        }
    }

    getIcon(id: number) {
        return UnitQueryService.getIcon(id);
    }
}

export class UnitPickerParams {
    model: any;
    allowGenerics: boolean;
}

ValidationRules
    .ensure((x: TeamGenericSlotEditorModel) => x.position)
    .satisfies((x: number, m: TeamGenericSlotEditorModel) => m.class + m.role + m.type > 0)
    .withMessage('You must specify something about this generic unit. Please add a class, type, or role.')
    .on(TeamGenericSlotEditorModel);