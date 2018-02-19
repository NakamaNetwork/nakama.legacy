import { autoinject, bindable, computedFrom, BindingEngine } from 'aurelia-framework';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { BoxQueryService, BoxEditorModel } from '../../services/query/box-query-service';
import { AlertService } from '../../services/alert-service';
import { DialogService } from 'aurelia-dialog';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import { Router } from 'aurelia-router';
import { AlertDialog } from '../../custom-elements/dialogs/alert-dialog';
import { AccountService } from '../../services/account-service';
import { BoxService } from '../../services/box-service';

@autoinject
export class BoxEditPage {
    public static nameMinLength = 5;
    public static nameMaxLength = 250;

    private dialogService: DialogService;
    private alertService: AlertService;
    private router: Router;
    private boxQueryService: BoxQueryService;
    private controller: ValidationController;
    private boxService: BoxService;

    private title: string = 'Create Box';
    private box: BoxEditorModel;
    private loading: boolean;

    constructor(boxQueryService: BoxQueryService,
        router: Router,
        alertService: AlertService,
        dialogService: DialogService,
        validFactory: ValidationControllerFactory,
        boxService: BoxService
    ) {
        this.controller = validFactory.createForCurrentScope();
        this.controller.addRenderer(new BeauterValidationFormRenderer());
        this.router = router;
        this.alertService = alertService;
        this.dialogService = dialogService;
        this.boxService = boxService;

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
        } else {
            if (this.boxService.reachedLimit) {
                this.router.navigateToRoute('error', { error: 'You have reached the maximum number of boxes allowed on this account (' + this.boxService.boxLimit + ').' });
            }
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
                } else {
                    x.results.filter(y => !y.valid && y.message).forEach(y => {
                        this.alertService.danger(y.message);
                    });
                }
            });
        }
    }

    doSubmit() {
        var updating = this.box.id;
        var item = this.box;
        item.friendId = item.friendId ? parseInt(item.friendId.toString().replace(/\D/g, '')) : item.friendId;
        this.boxQueryService.save(item).then(results => {
            if (!updating) {
                this.boxService.boxCount++;
                if (this.boxService.currentBox == null) {
                    this.boxService.setBox(results.id);
                }
            }
            this.alertService.success('Successfully saved ' + this.box.name + '!');
            this.router.navigateToRoute('boxDetails', { id: results.id });
        }).catch(response => this.alertService.reportError(response));
    }

    doDelete() {
        var message = 'Are you sure you want to delete this box? This cannot be undone!';
        this.dialogService.open({ viewModel: AlertDialog, model: { message: message, cancelable: true }, lock: true }).whenClosed(x => {
            if (!x.wasCancelled) {
                if (this.boxService.currentBox && this.boxService.currentBox.id === this.box.id) {
                    this.boxService.setBox(null, true)
                        .then(x => this.finalizeDelete())
                        .catch(response => this.alertService.reportError(response));
                } else {
                    this.finalizeDelete();
                }
            }
        });
    }

    finalizeDelete() {
        return this.boxQueryService.delete(this.box.id).then(results => {
            this.alertService.success('Successfully deleted box.');
            this.router.navigateToRoute('boxes');
            this.boxService.boxCount--;
        }).catch(response => this.alertService.reportError(response));
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