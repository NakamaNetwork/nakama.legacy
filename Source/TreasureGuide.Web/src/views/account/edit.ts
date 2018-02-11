import { autoinject, bindable, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { ProfileQueryService, ProfileEditorModel } from '../../services/query/profile-query-service';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { IProfileEditorModel } from '../../models/imported';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import { AlertService } from '../../services/alert-service';
import { AccountService } from '../../services/account-service';
import { DialogService } from 'aurelia-dialog';
import { AlertDialog } from '../../custom-elements/dialogs/alert-dialog';

@autoinject
export class ProfileEditPage {
    public static websiteMaxLength = 200;

    private profileQueryService: ProfileQueryService;
    private router: Router;
    private alert: AlertService;
    private accountService: AccountService;
    private dialogService: DialogService;

    public controller: ValidationController;

    private initialRoles: string;

    title = 'Edit Profile';
    @bindable profile: IProfileEditorModel;
    loading: boolean;

    constructor(profileQueryService: ProfileQueryService,
        router: Router,
        alertService: AlertService,
        validFactory: ValidationControllerFactory,
        accountService: AccountService,
        dialogService: DialogService
    ) {
        this.controller = validFactory.createForCurrentScope();
        this.controller.addRenderer(new BeauterValidationFormRenderer());

        this.profileQueryService = profileQueryService;
        this.accountService = accountService;
        this.dialogService = dialogService;
        this.router = router;
        this.alert = alertService;

        this.profile = new ProfileEditorModel();
        this.controller.addObject(this.profile);
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.loading = true;
            this.profileQueryService.editor(id).then(result => {
                this.title = 'Edit Profile';
                this.profile = Object.assign(this.profile, result);
                this.controller.validate();
                this.initialRoles = JSON.stringify(this.profile.userRoles);
                this.loading = false;
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested profile could not be found for editing. It may not exist or you may not have permission to edit it.' });
            });
        }
        this.controller.validate();
    }

    submit() {
        this.controller.validate().then(x => {
            if (x.valid) {
                var rolesChanged = this.initialRoles != JSON.stringify(this.profile.userRoles);
                if (rolesChanged) {
                    var message = 'Are you sure you want to set ' + this.profile.userName + '\'s roles to ' + this.profile.userRoles.join(', ') + '?';
                    this.dialogService.open({ viewModel: AlertDialog, model: { message: message, cancelable: true }, lock: true }).whenClosed(x => {
                        if (!x.wasCancelled) {
                            this.doSubmit();
                        }
                    });
                } else {
                    this.doSubmit();
                }
            } else {
                x.results.filter(y => !y.valid && y.message).forEach(y => {
                    this.alert.danger(y.message);
                });
            }
        });
    }

    doSubmit() {
        var item = this.profile;
        this.profileQueryService.save(item).then(results => {
            this.alert.success('Successfully updated profile information!');
            this.router.navigateToRoute('account', { id: results.id });
        }).catch(response => this.alert.reportError(response));
    }

    @computedFrom('profile.website')
    get webLength() {
        return (this.profile.website || '').length + '/' + ProfileEditPage.websiteMaxLength;
    }

    allRoles = AccountService.allRoles;
}

ValidationRules
    .ensure((x: ProfileEditorModel) => x.website)
    .maxLength(ProfileEditPage.websiteMaxLength)
    .on(ProfileEditorModel);