import { autoinject, BindingEngine } from 'aurelia-framework';
import { DonationQueryService, DonationSearchModel } from '../../services/query/donation-query-service';
import { IDonationStubModel } from '../../models/imported';
import { AccountService } from '../../services/account-service';

@autoinject
export class DonationHistoryPage {
    private donationQueryService: DonationQueryService;
    private bindingEngine: BindingEngine;
    private accountService: AccountService;

    private searchModel: DonationSearchModel;
    private loading: boolean;
    private donations: IDonationStubModel[] = [];

    constructor(donationQueryService: DonationQueryService, accountService: AccountService, bindingEngine: BindingEngine) {
        this.donationQueryService = donationQueryService;
        this.bindingEngine = bindingEngine;
        this.accountService = accountService;
    }

    activate() {
        this.searchModel = new DonationSearchModel();
        this.searchModel.complex = true;
        this.bindingEngine.propertyObserver(this.searchModel, 'payload').subscribe((n, o) => {
            this.search(n);
        });
        this.search(this.searchModel.payload);
    }

    search(payload) {
        if (this.donationQueryService) {
            this.loading = true;
            this.donationQueryService.search(payload).then(x => {
                this.donations = x.results;
                this.searchModel.totalResults = x.totalResults;
                this.loading = false;
            }).catch((e) => {
                this.loading = false;
            });
        }
    }
}