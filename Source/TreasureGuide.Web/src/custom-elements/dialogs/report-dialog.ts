import { autoinject, computedFrom } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import {AlertService} from '../../services/alert-service';

@autoinject
export class ReportDialog {
    private controller: DialogController;
    private validController: ValidationController;
    private alertService: AlertService;

    model: ReportDialogViewModel;

    constructor(controller: DialogController, validFactory: ValidationControllerFactory, alertService: AlertService) {
        this.controller = controller;
        this.validController = validFactory.createForCurrentScope();
        this.validController.addRenderer(new BeauterValidationFormRenderer());
        this.model = new ReportDialogViewModel();
        this.alertService = alertService;
    }

    okay() {
        this.validController.validate().then(x => {
            if (x.valid) {
                this.controller.ok(this.model.reason);
            } else {
                x.results.filter(y => !y.valid && y.message).forEach(y => {
                    this.alertService.danger(y.message);
                });
            }
        });
    };

    cancel() {
        this.controller.cancel();
    };

    @computedFrom('model.reason')
    get reasonLength() {
        return (this.model.reason || '').length + '/' + ReportDialogViewModel.reasonMaxLength;
    }
}

export class ReportDialogViewModel {
    public static reasonMaxLength: number = 100;
    reason: string;
}

ValidationRules
    .ensure((x: ReportDialogViewModel) => x.reason)
    .required()
    .maxLength(ReportDialogViewModel.reasonMaxLength)
    .on(ReportDialogViewModel);