import { autoinject } from 'aurelia-dependency-injection';
import { StageQueryService } from '../../services/query/stage-query-service';

@autoinject
export class StageIndexPage {
    title = 'Stages';
    stages = [];

    constructor(stageQueryService: StageQueryService) {
        stageQueryService.stub().then(results => {
            this.stages = results;
        });
    }
}