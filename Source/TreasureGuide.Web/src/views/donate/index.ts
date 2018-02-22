import { autoinject, computedFrom } from 'aurelia-framework';
import { AccountService } from '../../services/account-service';
import { RoleConstants } from '../../models/imported';
import { DialogService } from 'aurelia-dialog';
import { DonationQueryService } from '../../services/query/donation-query-service';
import { AlertDialog } from '../../custom-elements/dialogs/alert-dialog';
import { BoxConstants } from '../../models/imported';
import { BeauterValidationFormRenderer } from '../../renderers/beauter-validation-form-renderer';
import { DonationSubmissionModel } from '../../services/query/donation-query-service';
import { ValidationControllerFactory, ValidationController, ValidationRules } from 'aurelia-validation';
import { Router } from 'aurelia-router';

@autoinject
export class DonateIndexPage {
    private accountService: AccountService;
    private dialogService: DialogService;
    private donationQueryService: DonationQueryService;
    private controller: ValidationController;
    private router: Router;

    public model: DonationSubmissionModel;

    private processing: boolean;

    perks = [
        'More boxes! From <strong>' + BoxConstants.BoxLimit + '</strong> to <strong>' + BoxConstants.DonorBoxLimit + '!</strong>',
        'A shiny user name display!',
        'Lifetime access to any and all additional perks added in the future.'
    ];

    constructor(
        accountService: AccountService,
        dialogService: DialogService,
        donationQueryService: DonationQueryService,
        validFactory: ValidationControllerFactory,
        router: Router
    ) {
        this.accountService = accountService;
        this.dialogService = dialogService;
        this.donationQueryService = donationQueryService;
        this.controller = validFactory.createForCurrentScope();
        this.controller.addRenderer(new BeauterValidationFormRenderer());

        this.model = new DonationSubmissionModel();
        this.controller.addObject(this.model);
        this.router = router;
    }

    @computedFrom('accountService.isLoggedIn')
    get loggedIn() {
        return this.accountService.isLoggedIn;
    }

    @computedFrom('accountService.userProfile.userRoles')
    get alreadyDonated() {
        return this.accountService.isInRoles(RoleConstants.Donor);
    }

    @computedFrom('model.message')
    get messageLength() {
        return (this.model.message || '').length + '/' + DonationSubmissionModel.messageMaxLength;
    }

    submit() {
        return this.checkLoggedIn();
    }

    checkLoggedIn() {
        if (!this.loggedIn) {
            var message = 'You\'re not logged in! If you donate without logging in you won\'t be able to claim your donor perks. Are you sure you want to continue?';
            this.dialogService.open({ viewModel: AlertDialog, model: { message: message, cancelable: true }, lock: true }).whenClosed(x => {
                if (!x.wasCancelled) {
                    return this.checkDonated();
                }
            });
        }
        return this.checkDonated();
    }

    checkDonated() {
        if (this.alreadyDonated) {
            var message = 'You\'ve already donated and have lifetime access to all perks including any perks that are added in a later update. Are you sure you want to continue?';
            this.dialogService.open({ viewModel: AlertDialog, model: { message: message, cancelable: true }, lock: true }).whenClosed(x => {
                if (!x.wasCancelled) {
                    return this.verify();
                }
            });
        }
        return this.verify();
    }

    verify() {
        var message = 'You will be redirected to PayPal to fulfil your donation of $' + this.model.amount + '.';
        this.dialogService.open({ viewModel: AlertDialog, model: { message: message, cancelable: true }, lock: true }).whenClosed(x => {
            if (!x.wasCancelled) {
                return this.doSubmit();
            }
        });
    }

    doSubmit() {
        this.processing = true;
        this.donationQueryService.prepare(this.model).then(x => {
            if (x.error) {
                this.router.navigateToRoute('error', { error: x.error });
            } else {
                window.location.href = x.redirectUrl;
            }
        }).catch(x => {
            this.router.navigateToRoute('error', { error: x });
        });
    }
}

ValidationRules
    .ensure((x: DonationSubmissionModel) => x.message)
    .minLength(DonationSubmissionModel.messageMaxLength)
    .ensure((x: DonationSubmissionModel) => x.amount)
    .required()
    .satisfies(a => a <= DonationSubmissionModel.max && a >= DonationSubmissionModel.min)
    .withMessage(`Please enter an amount between $${DonationSubmissionModel.min} and $${DonationSubmissionModel.max}.`)
    .on(DonationSubmissionModel);