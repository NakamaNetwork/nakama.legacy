import { autoinject } from 'aurelia-dependency-injection';
import { StageQueryService } from '../../services/query/stage-query-service';

@autoinject
export class StageIndexPage {
    title = 'Stages';
    teams = [];

    constructor(stageQueryService: StageQueryService) {
        stageQueryService.stub().then(results => {
            this.teams = results;
        });
    }
}