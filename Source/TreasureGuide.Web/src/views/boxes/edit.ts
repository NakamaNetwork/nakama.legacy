import { autoinject, bindable, computedFrom, BindingEngine } from 'aurelia-framework';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BoxQueryService, BoxEditorModel } from '../../services/query/box-query-service';
import { AlertService } from '../../services/alert-service';
import { DialogService } from 'aurelia-dialog';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import { Router } from 'aurelia-router';
import { AlertDialog } from '../../custom-elements/dialogs/alert-dialog';

@autoinject
export class BoxEditPage {
    public static nameMinLength = 5;
    public static nameMaxLength = 250;

    private dialogService: DialogService;
    private alertService: AlertService;
    private router: Router;
    private boxQueryService: BoxQueryService;
    private controller: ValidationController;

    private title: string = 'Create Box';
    private box: BoxEditorModel;
    private loading: boolean;

    constructor(boxQueryService: BoxQueryService,
        router: Router,
        alertService: AlertService,
        dialogService: DialogService,
        validFactory: ValidationControllerFactory
    ) {
        this.controller = validFactory.createForCurrentScope();
        this.controller.addRenderer(new BeauterValidationFormRenderer());
        this.router = router;
        this.alertService = alertService;
        this.dialogService = dialogService;

        this.boxQueryService = boxQueryService;
        this.box = new BoxEditorModel();
        this.controller.addObject(this.box);
    }


    activate(params) {
        var id = params.id;
        if (id) {
            this.loading = true;
            this.boxQueryService.editor(id).then(result => {
                this.title = 'Edit Box';
                this.box = Object.assign(this.box, result);
                this.controller.validate();
                this.loading = false;
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested box could not be found for editing. It may not exist or you may not have permission to edit it.' });
            });
        }
        this.controller.validate();
    }

    @computedFrom('box.name')
    get nameLength() {
        return (this.box.name || '').length + '/' + BoxEditPage.nameMaxLength;
    }

    submit() {
        if (this.box.deleted) {
            this.doDelete();
        } else {
            this.controller.validate().then(x => {
                if (x.valid) {
                    this.doSubmit();
                }
            });
        }
    }

    doSubmit() {
        this.boxQueryService.save(this.box).then(results => {
            this.alertService.success('Successfully saved ' + this.box.name + '!');
            this.router.navigateToRoute('boxDetails', { id: results.id });
        }).catch(response => {
            return response.text().then(msg => {
                if (msg) {
                    this.alertService.danger(msg);
                } else {
                    this.alertService.danger('An error has occurred. Please try again in a few moments.');
                }
            }).catch(error => {
                this.alertService.danger('An error has occurred. Please try again in a few moments.');
            });
        });
    }

    doDelete() {
        var message = 'Are you sure you want to delete this box? This cannot be undone!';
        this.dialogService.open({ viewModel: AlertDialog, model: { message: message, cancelable: true }, lock: true }).whenClosed(x => {
            if (!x.wasCancelled) {
                this.boxQueryService.delete(this.box.id).then(results => {
                    this.alertService.success('Successfully deleted box.');
                    this.router.navigateToRoute('boxes');
                }).catch(response => {
                    return response.text().then(msg => {
                        if (msg) {
                            this.alertService.danger(msg);
                        } else {
                            this.alertService.danger('An error has occurred. Please try again in a few moments.');
                        }
                    }).catch(error => {
                        this.alertService.danger('An error has occurred. Please try again in a few moments.');
                    });
                });
            }
        });
    }
}

ValidationRules
    .ensure((x: BoxEditorModel) => x.name)
    .required()
    .minLength(BoxEditPage.nameMinLength)
    .maxLength(BoxEditPage.nameMaxLength)
    .ensure((x: BoxEditorModel) => x.friendId)
    .satisfies(x => x ? x.toString().replace(/\D/g, '').length === 9 : true)
    .withMessage('Friend Id must be a valid 9-digit number!')
    .on(BoxEditorModel);