import { autoinject, computedFrom, BindingEngine } from 'aurelia-framework';
import { DonationQueryService, DonationSearchModel } from '../../services/query/donation-query-service';
import { IDonationStubModel } from '../../models/imported';

@autoinject
export class SupportIndexPage {
    private donationQueryService: DonationQueryService;
    private bindingEngine: BindingEngine;

    private searchModel: DonationSearchModel;
    private loading: boolean;
    private donations: IDonationStubModel[] = [];

    constructor(donationQueryService: DonationQueryService, bindingEngine: BindingEngine) {
        this.donationQueryService = donationQueryService;
        this.bindingEngine = bindingEngine;
    }

    activate() {
        this.searchModel = new DonationSearchModel();
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