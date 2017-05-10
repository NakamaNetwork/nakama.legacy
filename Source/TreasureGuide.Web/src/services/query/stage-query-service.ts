import { autoinject } from 'aurelia-dependency-injection';
import { HttpEngine } from '../../tools/http-engine';
import { GenericApiQueryService } from './generic/generic-query-service';

@autoinject
export class StageQueryService extends GenericApiQueryService {
    constructor(http: HttpEngine) {
        super('stage', http);
    }
}