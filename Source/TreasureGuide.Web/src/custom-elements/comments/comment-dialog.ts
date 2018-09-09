import { autoinject, computedFrom } from 'aurelia-framework';
import { DialogController } from 'aurelia-dialog';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import { AlertService } from '../../services/alert-service';
import { ITeamCommentEditorModel } from '../../models/imported';

@autoinject
export class CommentDialog {
    private controller: DialogController;
    private validController: ValidationController;
    private alertService: AlertService;

    model: CommentDialogViewModel;
    name: string = 'Submit Comment';

    constructor(controller: DialogController, validFactory: ValidationControllerFactory, alertService: AlertService) {
        this.controller = controller;
        this.validController = validFactory.createForCurrentScope();
        this.validController.addRenderer(new BeauterValidationFormRenderer());
        this.model = new CommentDialogViewModel();
        this.alertService = alertService;
    }

    activate(model: ITeamCommentEditorModel) {
        if (model) {
            this.model.text = model.text;
            this.name = 'Edit Comment';
        }
    }

    okay() {
        this.validController.validate().then(x => {
            if (x.valid) {
                this.controller.ok(this.model.text);
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

    @computedFrom('model.text')
    get textLength() {
        return (this.model.text || '').length + '/' + CommentDialogViewModel.textMaxLength;
    }
}

export class CommentDialogViewModel {
    public static textMaxLength: number = 4000;
    text: string;
}

ValidationRules
    .ensure((x: CommentDialogViewModel) => x.text)
    .required()
    .maxLength(CommentDialogViewModel.textMaxLength)
    .on(CommentDialogViewModel);