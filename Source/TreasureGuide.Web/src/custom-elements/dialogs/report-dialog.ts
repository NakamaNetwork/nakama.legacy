import { autoinject, computedFrom } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';

@autoinject
export class ReportDialog {
    private controller: DialogController;
    private validController: ValidationController;

    model: ReportDialogViewModel;

    constructor(controller: DialogController, validFactory: ValidationControllerFactory) {
        this.controller = controller;
        this.validController = validFactory.createForCurrentScope();
        this.validController.addRenderer(new BeauterValidationFormRenderer());
        this.model = new ReportDialogViewModel();
    }

    okay() {
        this.controller.ok(this.model.reason);
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