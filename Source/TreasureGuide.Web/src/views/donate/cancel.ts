import { autoinject, computedFrom } from 'aurelia-framework';
import { AccountService } from '../../services/account-service';
import { DonationQueryService } from '../../services/query/donation-query-service';
import { Router } from 'aurelia-router';
import { IDonationVerificationModel } from '../../models/imported';
import { AlertDialog } from '../../custom-elements/dialogs/alert-dialog';

@autoinject
export class CancelDonationPage {
    private accountService: AccountService;
    private donationQueryService: DonationQueryService;
    private router: Router;

    private processing: boolean;

    constructor(
        accountService: AccountService,
        donationQueryService: DonationQueryService,
        router: Router
    ) {
        this.accountService = accountService;
        this.donationQueryService = donationQueryService;
    }

    activate(params) {
        var model = <IDonationVerificationModel>{
            payerId: params.PayerID,
            paymentId: params.paymentId
        };

        this.donationQueryService.finalize(model).then(x => {
            this.router.navigateToRoute('donate/complete', x);
        }).catch(x => {
            this.router.navigateToRoute('error', { error: x });
        });
    }
}