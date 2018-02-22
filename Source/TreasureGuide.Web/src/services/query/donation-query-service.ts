import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { SearchableQueryService } from './generic/searchable-query-service';
import { SearchModel } from '../../models/search-model';
import { IDonationFinalizationModel, IDonationSearchModel, IDonationSubmissionModel } from '../../models/imported';

@autoinject
export class DonationQueryService extends SearchableQueryService {
    constructor(http: HttpEngine) {
        super('donation', http, true);
    }

    prepare(model: IDonationSubmissionModel): Promise<number> {
        return this.http.post(this.buildAddress('prepare'), model);
    }

    finalize(model: IDonationFinalizationModel): Promise<string> {
        return this.http.post(this.buildAddress('finalize'), model);
    }

    abort(model: IDonationFinalizationModel): Promise<string> {
        return this.http.post(this.buildAddress('abort'), model);
    }
}

export class DonationSearchModel extends SearchModel implements IDonationSearchModel {
    user: string;
    startDate: Date;
    endDate: Date;
    minAmount: number;
    maxAmount: number;

    getDefault(): SearchModel {
        return new DonationSearchModel();
    }
}