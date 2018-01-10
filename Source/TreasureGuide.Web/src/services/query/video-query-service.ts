import { autoinject } from 'aurelia-framework';
import { HttpEngine } from '../../tools/http-engine';
import { GenericApiQueryService } from './generic/generic-api-query-service';
import { ITeamVideoModel } from '../../models/imported';

@autoinject
export class VideoQueryService extends GenericApiQueryService {
    constructor(http: HttpEngine) {
        super('video', http, false);
    }

    save(model: ITeamVideoModel, id?): Promise<any> {
        return super.save(model, id);
    }
}