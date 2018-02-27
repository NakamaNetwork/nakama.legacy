import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { SearchModel } from '../../models/search-model';
import { IDonationVerificationModel, IDonationSearchModel, IDonationSubmissionModel, IDonationResultModel, PaymentType } from '../../models/imported';
import { SearchConstants } from '../../models/imported';

@autoinject
export class DonationQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('donation', http, true);
    }

    prepare(model: IDonationSubmissionModel): Promise<IDonationResultModel> {
        return this.http.post(this.buildAddress('prepare'), model);
    }

    refresh(model: IDonationVerificationModel): Promise<IDonationResultModel> {
        return this.http.post(this.buildAddress('refresh'), model);
    }

    refreshAll(m: Promise<IDonationResultModel[]>) {
        return this.http.post(this.buildAddress('refreshAll'));
    }

    cancel(model: IDonationVerificationModel): Promise<IDonationResultModel> {
        return this.http.post(this.buildAddress('cancel'), model);
    }
}

export class DonationSearchModel extends SearchModel implements IDonationSearchModel {
    user: string;
    startDate: Date;
    endDate: Date;
    minAmount: number;
    maxAmount: number;
    complex: boolean;

    getDefault(): SearchModel {
        return new DonationSearchModel();
    }

    sortables: string[] = [
        SearchConstants.SortUser,
        SearchConstants.SortAmount,
        SearchConstants.SortDate
    ];
}

export class DonationSubmissionModel implements IDonationSubmissionModel {
    public static messageMaxLength = 500;
    public static min = 1.00;
    public static max = 500.00;

    public amount: number = 5.00;
    public paymentType: PaymentType = PaymentType.Paypal;
    public message: string;
    public public: boolean;
}