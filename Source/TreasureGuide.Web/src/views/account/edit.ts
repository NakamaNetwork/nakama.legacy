import { autoinject, bindable, computedFrom } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { ProfileQueryService, ProfileEditorModel } from '../../services/query/profile-query-service';
import { ValidationControllerFactory, ValidationRules, ValidationController } from 'aurelia-validation';
import { IProfileEditorModel } from '../../models/imported';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import { AlertService } from '../../services/alert-service';

@autoinject
export class ProfileEditPage {
    public static nameMinLength = 10;
    public static nameMaxLength = 250;
    public static websiteMaxLength = 200;

    private profileQueryService: ProfileQueryService;
    private router: Router;
    private alert: AlertService;

    public controller: ValidationController;

    title = 'Edit Profile';
    @bindable profile: IProfileEditorModel;

    constructor(profileQueryService: ProfileQueryService, router: Router, alertService: AlertService, validFactory: ValidationControllerFactory) {
        this.controller = validFactory.createForCurrentScope();
        this.controller.addRenderer(new BeauterValidationFormRenderer());

        this.profileQueryService = profileQueryService;
        this.router = router;
        this.alert = alertService;

        this.profile = new ProfileEditorModel();
        this.controller.addObject(this.profile);
    }

    activate(params) {
        var id = params.id;
        if (id) {
            this.profileQueryService.editor(id).then(result => {
                this.title = 'Edit Profile';
                this.profile = Object.assign(this.profile, result);
                this.controller.validate();
            }).catch(error => {
                this.router.navigateToRoute('error', { error: 'The requested profile could not be found for editing. It may not exist or you may not have permission to edit it.' });
            });
        }
        this.controller.validate();
    }

    submit() {
        this.controller.validate().then(x => {
            if (x.valid) {
                var item = this.profile;
                item.accountNumber = parseInt(item.accountNumber.toString().replace(/\D/g, ''));
                this.profileQueryService.save(item).then(results => {
                    this.alert.success('Successfully updated profile information!');
                    this.router.navigateToRoute('account', { id: results.id });
                }).catch(response => {
                    return response.text().then(msg => {
                        this.alert.danger(msg);
                    }).catch(error => {
                        this.alert.danger('An error has occurred. Please try again in a few moments.');
                    });
                });
            } else {
                x.results.filter(y => !y.valid && y.message).forEach(y => {
                    this.alert.danger(y.message);
                });
            }
        });
    }

    @computedFrom('profile.userName')
    get nameLength() {
        return (this.profile.userName || '').length + '/' + ProfileEditPage.nameMaxLength;
    }

    @computedFrom('profile.website')
    get webLength() {
        return (this.profile.website || '').length + '/' + ProfileEditPage.websiteMaxLength;
    }
}

ValidationRules
    .ensure((x: ProfileEditorModel) => x.userName)
    .required()
    .minLength(ProfileEditPage.nameMinLength)
    .maxLength(ProfileEditPage.nameMaxLength)
    .ensure((x: ProfileEditorModel) => x.accountNumber)
    .satisfies(x => x.toString().replace(/\D/g, '').length === 9)
    .withMessage('Friend Id must be a valid 9-digit string!')
    .ensure((x: ProfileEditorModel) => x.website)
    .maxLength(ProfileEditPage.websiteMaxLength)
    .on(ProfileEditorModel);