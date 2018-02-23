import { autoinject } from 'aurelia-framework';
import { AccountService } from '../../services/account-service';
import { DonationQueryService } from '../../services/query/donation-query-service';
import { Router } from 'aurelia-router';
import { IDonationVerificationModel, IDonationResultModel } from '../../models/imported';
import { PaymentState } from '../../models/imported';

@autoinject
export class CancelDonationPage {
    private accountService: AccountService;
    private donationQueryService: DonationQueryService;
    private router: Router;

    private processing: boolean;
    private result: IDonationResultModel;

    constructor(
        accountService: AccountService,
        donationQueryService: DonationQueryService,
        router: Router
    ) {
        this.accountService = accountService;
        this.donationQueryService = donationQueryService;
        this.router = router;
    }

    activate(params) {
        var model = <IDonationVerificationModel>{
            tokenId: params.token
        };

        if (model.tokenId) {
            this.cancel(model);
        } else {
            this.router.navigateToRoute('error', { error: 'Could not retrieve your donation. Please contact the administrator for assistance.' });
        }
    }

    cancel(model: IDonationVerificationModel) {
        this.processing = true;
        this.donationQueryService.cancel(model).then(x => {
            if (x.error) {
                this.router.navigateToRoute('error', { error: x.error });
            } else if (x.state === PaymentState.Cancelled) {
                this.result = x;
                this.processing = false;
            } else {
                setTimeout(() => {
                    this.cancel(model);
                }, 5000);
            }
        }).catch(x => {
            setTimeout(() => {
                this.cancel(model);
            }, 5000);
        });
    }
}