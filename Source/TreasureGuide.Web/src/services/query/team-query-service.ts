import { autoinject } from 'aurelia-dependency-injection';
import { GenericApiQueryService } from './generic/generic-api-query-service';
import { HttpEngine } from '../../tools/http-engine';

@autoinject
export class TeamQueryService extends GenericApiQueryService {
    constructor(http: HttpEngine) {
        super('team', http);
    }
}