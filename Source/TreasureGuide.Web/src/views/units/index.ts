import { autoinject } from 'aurelia-dependency-injection';
import { UnitQueryService } from '../../services/query/unit-query-service';

@autoinject
export class UnitIndexPage {
    title = 'Units';
    units = [];

    constructor(unitQueryService: UnitQueryService) {
        unitQueryService.stub().then(results => {
            this.units = results;
        });
    }
}